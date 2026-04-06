using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models.Entities;
using SchoolManagement.Repositories.Interfaces;

namespace SchoolManagement.Repositories.Implementations;

public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
{
    public AttendanceRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Attendance>> GetByStudentIdAsync(int studentId)
    {
        return await DbSet
            .Where(a => a.StudentId == studentId && !a.IsDeleted)
            .Include(a => a.Student)
            .OrderByDescending(a => a.AttendanceDate)
            .ToListAsync();
    }

    public async Task<List<Attendance>> GetByDateRangeAsync(int studentId, DateTime startDate, DateTime endDate)
    {
        return await DbSet
            .Where(a => a.StudentId == studentId && 
                   a.AttendanceDate.Date >= startDate.Date && 
                   a.AttendanceDate.Date <= endDate.Date &&
                   !a.IsDeleted)
            .OrderBy(a => a.AttendanceDate)
            .ToListAsync();
    }

    public async Task<Attendance?> GetByStudentAndDateAsync(int studentId, DateTime date)
    {
        return await DbSet
            .FirstOrDefaultAsync(a => a.StudentId == studentId && 
                                 a.AttendanceDate.Date == date.Date &&
                                 !a.IsDeleted);
    }

    public async Task<AttendanceSummary> GetAttendanceSummaryAsync(int studentId, DateTime startDate, DateTime endDate)
    {
        var records = await GetByDateRangeAsync(studentId, startDate, endDate);
        
        return new AttendanceSummary
        {
            TotalDays = records.Count,
            PresentDays = records.Count(r => r.Status == AttendanceStatus.Present),
            AbsentDays = records.Count(r => r.Status == AttendanceStatus.Absent),
            LateDays = records.Count(r => r.Status == AttendanceStatus.Late),
            ExcusedDays = records.Count(r => r.Status == AttendanceStatus.Excused)
        };
    }
}
