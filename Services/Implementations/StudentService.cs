using Microsoft.EntityFrameworkCore;
using SchoolManagement.DTOs.Request;
using SchoolManagement.DTOs.Response;
using SchoolManagement.Models.Entities;
using SchoolManagement.Repositories.Interfaces;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Services.Implementations;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IParentRepository _parentRepository;
    private readonly IStudentParentRepository _studentParentRepository;
    private readonly IGradeRepository _gradeRepository;
    private readonly IAttendanceRepository _attendanceRepository;

    public StudentService(
        IStudentRepository studentRepository,
        IUserRepository userRepository,
        IParentRepository parentRepository,
        IStudentParentRepository studentParentRepository,
        IGradeRepository gradeRepository,
        IAttendanceRepository attendanceRepository)
    {
        _studentRepository = studentRepository;
        _userRepository = userRepository;
        _parentRepository = parentRepository;
        _studentParentRepository = studentParentRepository;
        _gradeRepository = gradeRepository;
        _attendanceRepository = attendanceRepository;
    }

    public async Task<StudentResponse?> GetStudentByIdAsync(int id)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        if (student == null)
            return null;

        var user = await _userRepository.GetByIdAsync(student.UserId);
        return MapToStudentResponse(student, user);
    }

    public async Task<StudentDetailResponse?> GetStudentDetailAsync(int id)
    {
        var student = await _studentRepository.GetStudentWithDetailsAsync(id);
        if (student == null)
            return null;

        var user = await _userRepository.GetByIdAsync(student.UserId);
        var averageGrade = await _gradeRepository.GetAverageGradeByStudentAsync(id);
        var attendanceSummary = await _attendanceRepository.GetAttendanceSummaryAsync(id, DateTime.MinValue, DateTime.MaxValue);

        var parents = student.StudentParents
            .Select(sp => new ParentResponse
            {
                Id = sp.Parent!.Id,
                FirstName = sp.Parent.User!.FirstName,
                LastName = sp.Parent.User.LastName,
                Email = sp.Parent.User.Email,
                PhoneNumber = sp.Parent.User.PhoneNumber ?? string.Empty,
                Relationship = sp.Relationship
            })
            .ToList();

        return new StudentDetailResponse
        {
            Id = student.Id,
            Email = user!.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            RollNumber = student.RollNumber,
            Grade = student.Grade,
            Section = student.Section,
            DateOfBirth = student.DateOfBirth,
            Address = student.Address,
            Parents = parents,
            AttendancePercentage = attendanceSummary.TotalDays > 0 
                ? (attendanceSummary.PresentDays * 100m) / attendanceSummary.TotalDays 
                : 0,
            AverageGrade = averageGrade
        };
    }

    public async Task<List<StudentResponse>> GetAllStudentsAsync()
    {
        var students = await _studentRepository.GetAllAsync();
        var responses = new List<StudentResponse>();

        foreach (var student in students)
        {
            var user = await _userRepository.GetByIdAsync(student.UserId);
            responses.Add(MapToStudentResponse(student, user));
        }

        return responses;
    }

    public async Task<List<StudentResponse>> GetStudentsByGradeAsync(string grade)
    {
        var students = await _studentRepository.GetStudentsByGradeAsync(grade);
        var responses = new List<StudentResponse>();

        foreach (var student in students)
        {
            var user = await _userRepository.GetByIdAsync(student.UserId);
            responses.Add(MapToStudentResponse(student, user));
        }

        return responses;
    }

    public async Task<List<StudentResponse>> GetStudentsPagedAsync(int pageNumber, int pageSize)
    {
        var students = await _studentRepository.GetPaginatedAsync(pageNumber, pageSize);
        var responses = new List<StudentResponse>();

        foreach (var student in students)
        {
            var user = await _userRepository.GetByIdAsync(student.UserId);
            responses.Add(MapToStudentResponse(student, user));
        }

        return responses;
    }

    public async Task<StudentResponse?> CreateStudentAsync(CreateStudentRequest request)
    {
        var user = new User
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = UserRole.Student,
            IsActive = true
        };

        var createdUser = await _userRepository.AddAsync(user);

        var student = new Student
        {
            UserId = createdUser.Id,
            RollNumber = request.RollNumber,
            Grade = request.Grade,
            Section = request.Section,
            DateOfBirth = request.DateOfBirth,
            Address = request.Address
        };

        var createdStudent = await _studentRepository.AddAsync(student);
        return MapToStudentResponse(createdStudent, createdUser);
    }

    public async Task<StudentResponse?> UpdateStudentAsync(int id, UpdateStudentRequest request)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        if (student == null)
            return null;

        student.Grade = request.Grade;
        student.Section = request.Section;
        student.DateOfBirth = request.DateOfBirth;
        student.Address = request.Address;
        student.UpdatedAt = DateTime.UtcNow;

        var user = await _userRepository.GetByIdAsync(student.UserId);
        user!.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);
        var updatedStudent = await _studentRepository.UpdateAsync(student);

        return MapToStudentResponse(updatedStudent, user);
    }

    public async Task<bool> DeleteStudentAsync(int id)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        if (student == null)
            return false;

        student.IsDeleted = true;
        student.UpdatedAt = DateTime.UtcNow;
        await _studentRepository.UpdateAsync(student);
        return true;
    }

    public async Task<bool> AssignParentAsync(int studentId, AssignParentRequest request)
    {
        var student = await _studentRepository.GetByIdAsync(studentId);
        if (student == null)
            return false;

        var parent = await _parentRepository.GetByIdAsync(request.ParentId);
        if (parent == null)
            return false;

        var existingAssignment = await _studentParentRepository.GetByStudentAndParentAsync(studentId, request.ParentId);
        if (existingAssignment != null)
            return false;

        var studentParent = new StudentParent
        {
            StudentId = studentId,
            ParentId = request.ParentId,
            Relationship = request.Relationship
        };

        await _studentParentRepository.AddAsync(studentParent);
        return true;
    }

    private StudentResponse MapToStudentResponse(Student student, User? user)
    {
        return new StudentResponse
        {
            Id = student.Id,
            UserId = student.UserId,
            Email = user?.Email ?? string.Empty,
            FirstName = user?.FirstName ?? string.Empty,
            LastName = user?.LastName ?? string.Empty,
            RollNumber = student.RollNumber,
            Grade = student.Grade,
            Section = student.Section,
            DateOfBirth = student.DateOfBirth,
            Address = student.Address
        };
    }
}
