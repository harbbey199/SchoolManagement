using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models.Entities;
using SchoolManagement.Repositories.Interfaces;

namespace SchoolManagement.Repositories.Implementations;

public class GradeRepository : Repository<Grade>, IGradeRepository
{
    public GradeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Grade>> GetByStudentIdAsync(int studentId)
    {
        return await DbSet
            .Where(g => g.StudentId == studentId && !g.IsDeleted)
            .Include(g => g.Student)
            .OrderByDescending(g => g.EvaluationDate)
            .ToListAsync();
    }

    public async Task<List<Grade>> GetByStudentAndTermAsync(int studentId, string term)
    {
        return await DbSet
            .Where(g => g.StudentId == studentId && 
                   g.Term == term &&
                   !g.IsDeleted)
            .Include(g => g.Student)
            .ToListAsync();
    }

    public async Task<decimal> GetAverageGradeByStudentAsync(int studentId)
    {
        var grades = await DbSet
            .Where(g => g.StudentId == studentId && !g.IsDeleted)
            .ToListAsync();

        if (!grades.Any())
            return 0;

        var percentages = grades.Select(g => (g.Marks / g.MaxMarks) * 100);
        return (decimal)percentages.Average();
    }
}
