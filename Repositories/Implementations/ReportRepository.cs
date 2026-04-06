using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models.Entities;
using SchoolManagement.Repositories.Interfaces;

namespace SchoolManagement.Repositories.Implementations;

public class ReportRepository : Repository<Report>, IReportRepository
{
    public ReportRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Report?> GetByStudentAndTermAsync(int studentId, string term)
    {
        return await DbSet
            .FirstOrDefaultAsync(r => r.StudentId == studentId && 
                                 r.Term == term &&
                                 !r.IsDeleted);
    }

    public async Task<List<Report>> GetByStudentAsync(int studentId)
    {
        return await DbSet
            .Where(r => r.StudentId == studentId && !r.IsDeleted)
            .Include(r => r.Student)
            .OrderByDescending(r => r.GeneratedDate)
            .ToListAsync();
    }
}
