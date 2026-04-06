using SchoolManagement.DTOs.Request;
using SchoolManagement.DTOs.Response;
using SchoolManagement.Models.Entities;
using SchoolManagement.Repositories.Interfaces;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Services.Implementations;

public class AttendanceService : IAttendanceService
{
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IUserRepository _userRepository;

    public AttendanceService(
        IAttendanceRepository attendanceRepository,
        IStudentRepository studentRepository,
        IUserRepository userRepository)
    {
        _attendanceRepository = attendanceRepository;
        _studentRepository = studentRepository;
        _userRepository = userRepository;
    }

    public async Task<AttendanceResponse?> MarkAttendanceAsync(MarkAttendanceRequest request)
    {
        var existingAttendance = await _attendanceRepository.GetByStudentAndDateAsync(
            request.StudentId, request.AttendanceDate);

        Attendance attendance;
        if (existingAttendance != null)
        {
            existingAttendance.Status = (AttendanceStatus)request.Status;
            existingAttendance.Remarks = request.Remarks;
            existingAttendance.UpdatedAt = DateTime.UtcNow;
            attendance = await _attendanceRepository.UpdateAsync(existingAttendance);
        }
        else
        {
            attendance = new Attendance
            {
                StudentId = request.StudentId,
                AttendanceDate = request.AttendanceDate,
                Status = (AttendanceStatus)request.Status,
                Remarks = request.Remarks
            };
            attendance = await _attendanceRepository.AddAsync(attendance);
        }

        return await MapToAttendanceResponseAsync(attendance);
    }

    public async Task<bool> MarkMultipleAttendanceAsync(MarkMultipleAttendanceRequest request)
    {
        try
        {
            var date = DateTime.UtcNow.Date;
            foreach (var record in request.Records)
            {
                await MarkAttendanceAsync(new MarkAttendanceRequest
                {
                    StudentId = record.StudentId,
                    AttendanceDate = date,
                    Status = record.Status,
                    Remarks = record.Remarks
                });
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<AttendanceResponse>> GetStudentAttendanceAsync(int studentId)
    {
        var records = await _attendanceRepository.GetByStudentIdAsync(studentId);
        var responses = new List<AttendanceResponse>();

        foreach (var record in records)
        {
            var response = await MapToAttendanceResponseAsync(record);
            if (response != null)
                responses.Add(response);
        }

        return responses;
    }

    public async Task<List<AttendanceResponse>> GetAttendanceByDateRangeAsync(
        int studentId, DateTime startDate, DateTime endDate)
    {
        var records = await _attendanceRepository.GetByDateRangeAsync(studentId, startDate, endDate);
        var responses = new List<AttendanceResponse>();

        foreach (var record in records)
        {
            var response = await MapToAttendanceResponseAsync(record);
            if (response != null)
                responses.Add(response);
        }

        return responses;
    }

    public async Task<AttendanceSummaryResponse?> GetAttendanceSummaryAsync(
        int studentId, DateTime startDate, DateTime endDate)
    {
        var summary = await _attendanceRepository.GetAttendanceSummaryAsync(studentId, startDate, endDate);
        var student = await _studentRepository.GetByIdAsync(studentId);
        var user = student != null ? await _userRepository.GetByIdAsync(student.UserId) : null;

        if (student == null || user == null)
            return null;

        return new AttendanceSummaryResponse
        {
            StudentId = studentId,
            StudentName = $"{user.FirstName} {user.LastName}",
            TotalDays = summary.TotalDays,
            PresentDays = summary.PresentDays,
            AbsentDays = summary.AbsentDays,
            LateDays = summary.LateDays,
            AttendancePercentage = summary.TotalDays > 0 
                ? (summary.PresentDays * 100m) / summary.TotalDays 
                : 0
        };
    }

    private async Task<AttendanceResponse?> MapToAttendanceResponseAsync(Attendance attendance)
    {
        var student = await _studentRepository.GetByIdAsync(attendance.StudentId);
        var user = student != null ? await _userRepository.GetByIdAsync(student.UserId) : null;

        if (user == null)
            return null;

        return new AttendanceResponse
        {
            Id = attendance.Id,
            StudentId = attendance.StudentId,
            StudentName = $"{user.FirstName} {user.LastName}",
            AttendanceDate = attendance.AttendanceDate,
            Status = attendance.Status.ToString(),
            Remarks = attendance.Remarks
        };
    }
}
