using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models.Entities;
using SchoolManagement.Repositories.Interfaces;

namespace SchoolManagement.Repositories.Implementations;

public class ParentRepository : Repository<Parent>, IParentRepository
{
    public ParentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Parent?> GetByUserIdAsync(int userId)
    {
        return await DbSet
            .FirstOrDefaultAsync(p => p.UserId == userId && !p.IsDeleted);
    }
}
