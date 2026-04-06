using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models.Entities;
using SchoolManagement.Repositories.Interfaces;

namespace SchoolManagement.Repositories.Implementations;

public class StudentRepository : Repository<Student>, IStudentRepository
{
    public StudentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Student?> GetStudentWithDetailsAsync(int id)
    {
        return await DbSet
            .Include(s => s.User)
            .Include(s => s.StudentParents)
            .ThenInclude(sp => sp.Parent)
            .ThenInclude(p => p!.User)
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
    }

    public async Task<List<Student>> GetStudentsByGradeAsync(string grade)
    {
        return await DbSet
            .Where(s => s.Grade == grade && !s.IsDeleted)
            .Include(s => s.User)
            .ToListAsync();
    }

    public async Task<Student?> GetByRollNumberAsync(string rollNumber)
    {
        return await DbSet.FirstOrDefaultAsync(s => s.RollNumber == rollNumber && !s.IsDeleted);
    }

    public async Task<List<Student>> GetPaginatedAsync(int pageNumber, int pageSize)
    {
        return await DbSet
            .Where(s => !s.IsDeleted)
            .Include(s => s.User)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}
