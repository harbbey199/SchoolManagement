using SchoolManagement.DTOs.Request;
using SchoolManagement.DTOs.Response;
using SchoolManagement.Models.Entities;
using SchoolManagement.Repositories.Interfaces;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Services.Implementations;

public class GradeService : IGradeService
{
    private readonly IGradeRepository _gradeRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IUserRepository _userRepository;

    public GradeService(
        IGradeRepository gradeRepository,
        IStudentRepository studentRepository,
        IUserRepository userRepository)
    {
        _gradeRepository = gradeRepository;
        _studentRepository = studentRepository;
        _userRepository = userRepository;
    }

    public async Task<GradeResponse?> RecordGradeAsync(RecordGradeRequest request)
    {
        var student = await _studentRepository.GetByIdAsync(request.StudentId);
        if (student == null)
            return null;

        var gradeValue = CalculateGradeValue(request.Marks, request.MaxMarks);

        var grade = new Grade
        {
            StudentId = request.StudentId,
            Subject = request.Subject,
            Marks = request.Marks,
            MaxMarks = request.MaxMarks,
            GradeValue = gradeValue,
            EvaluationDate = DateTime.UtcNow,
            Term = request.Term
        };

        var createdGrade = await _gradeRepository.AddAsync(grade);
        return await MapToGradeResponseAsync(createdGrade);
    }

    public async Task<List<GradeResponse>> GetStudentGradesAsync(int studentId)
    {
        var grades = await _gradeRepository.GetByStudentIdAsync(studentId);
        var responses = new List<GradeResponse>();

        foreach (var grade in grades)
        {
            var response = await MapToGradeResponseAsync(grade);
            if (response != null)
                responses.Add(response);
        }

        return responses;
    }

    public async Task<List<GradeResponse>> GetStudentGradesByTermAsync(int studentId, string term)
    {
        var grades = await _gradeRepository.GetByStudentAndTermAsync(studentId, term);
        var responses = new List<GradeResponse>();

        foreach (var grade in grades)
        {
            var response = await MapToGradeResponseAsync(grade);
            if (response != null)
                responses.Add(response);
        }

        return responses;
    }

    public async Task<bool> UpdateGradeAsync(int gradeId, RecordGradeRequest request)
    {
        var grade = await _gradeRepository.GetByIdAsync(gradeId);
        if (grade == null)
            return false;

        grade.Subject = request.Subject;
        grade.Marks = request.Marks;
        grade.MaxMarks = request.MaxMarks;
        grade.GradeValue = CalculateGradeValue(request.Marks, request.MaxMarks);
        grade.Term = request.Term;
        grade.UpdatedAt = DateTime.UtcNow;

        await _gradeRepository.UpdateAsync(grade);
        return true;
    }

    private string CalculateGradeValue(decimal marks, decimal maxMarks)
    {
        var percentage = (marks / maxMarks) * 100;
        return percentage switch
        {
            >= 90 => "A+",
            >= 80 => "A",
            >= 70 => "B",
            >= 60 => "C",
            >= 50 => "D",
            _ => "F"
        };
    }

    private async Task<GradeResponse?> MapToGradeResponseAsync(Grade grade)
    {
        var student = await _studentRepository.GetByIdAsync(grade.StudentId);
        var user = student != null ? await _userRepository.GetByIdAsync(student.UserId) : null;

        if (user == null)
            return null;

        return new GradeResponse
        {
            Id = grade.Id,
            StudentId = grade.StudentId,
            StudentName = $"{user.FirstName} {user.LastName}",
            Subject = grade.Subject,
            Marks = grade.Marks,
            MaxMarks = grade.MaxMarks,
            GradeValue = grade.GradeValue,
            EvaluationDate = grade.EvaluationDate,
            Term = grade.Term
        };
    }
}
