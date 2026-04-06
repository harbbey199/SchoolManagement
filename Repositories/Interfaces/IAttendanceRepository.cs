using SchoolManagement.Models.Entities;

namespace SchoolManagement.Repositories.Interfaces;

public interface IAttendanceRepository : IRepository<Attendance>
{
    Task<List<Attendance>> GetByStudentIdAsync(int studentId);
    Task<List<Attendance>> GetByDateRangeAsync(int studentId, DateTime startDate, DateTime endDate);
    Task<Attendance?> GetByStudentAndDateAsync(int studentId, DateTime date);
    Task<AttendanceSummary> GetAttendanceSummaryAsync(int studentId, DateTime startDate, DateTime endDate);
}

public class AttendanceSummary
{
    public int TotalDays { get; set; }
    public int PresentDays { get; set; }
    public int AbsentDays { get; set; }
    public int LateDays { get; set; }
    public int ExcusedDays { get; set; }
}
