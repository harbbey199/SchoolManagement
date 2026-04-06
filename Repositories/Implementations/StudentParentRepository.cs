using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models.Entities;
using SchoolManagement.Repositories.Interfaces;

namespace SchoolManagement.Repositories.Implementations;

public class StudentParentRepository : Repository<StudentParent>, IStudentParentRepository
{
    public StudentParentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<StudentParent>> GetByStudentIdAsync(int studentId)
    {
        return await DbSet
            .Where(sp => sp.StudentId == studentId && !sp.IsDeleted)
            .Include(sp => sp.Parent)
            .ThenInclude(p => p!.User)
            .ToListAsync();
    }

    public async Task<List<StudentParent>> GetByParentIdAsync(int parentId)
    {
        return await DbSet
            .Where(sp => sp.ParentId == parentId && !sp.IsDeleted)
            .Include(sp => sp.Student)
            .ThenInclude(s => s!.User)
            .ToListAsync();
    }

    public async Task<StudentParent?> GetByStudentAndParentAsync(int studentId, int parentId)
    {
        return await DbSet
            .FirstOrDefaultAsync(sp => sp.StudentId == studentId && 
                                 sp.ParentId == parentId &&
                                 !sp.IsDeleted);
    }
}
