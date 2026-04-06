using SchoolManagement.DTOs.Request;
using SchoolManagement.DTOs.Response;

namespace SchoolManagement.Services.Interfaces;

public interface IAttendanceService
{
    Task<AttendanceResponse?> MarkAttendanceAsync(MarkAttendanceRequest request);
    Task<bool> MarkMultipleAttendanceAsync(MarkMultipleAttendanceRequest request);
    Task<List<AttendanceResponse>> GetStudentAttendanceAsync(int studentId);
    Task<List<AttendanceResponse>> GetAttendanceByDateRangeAsync(int studentId, DateTime startDate, DateTime endDate);
    Task<AttendanceSummaryResponse?> GetAttendanceSummaryAsync(int studentId, DateTime startDate, DateTime endDate);
}
