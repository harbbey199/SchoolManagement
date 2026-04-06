using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models.Entities;
using SchoolManagement.Repositories.Interfaces;

namespace SchoolManagement.Repositories.Implementations;

public class PaymentRepository : Repository<Payment>, IPaymentRepository
{
    public PaymentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Payment>> GetByStudentIdAsync(int studentId)
    {
        return await DbSet
            .Where(p => p.StudentId == studentId && !p.IsDeleted)
            .Include(p => p.Student)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task<List<Payment>> GetByStatusAsync(PaymentStatus status)
    {
        return await DbSet
            .Where(p => p.Status == status && !p.IsDeleted)
            .Include(p => p.Student)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalByStudentAsync(int studentId)
    {
        return await DbSet
            .Where(p => p.StudentId == studentId && 
                   p.Status == PaymentStatus.Completed &&
                   !p.IsDeleted)
            .SumAsync(p => p.Amount);
    }

    public async Task<decimal> GetAmountByStudentAndStatusAsync(int studentId, PaymentStatus status)
    {
        return await DbSet
            .Where(p => p.StudentId == studentId && 
                   p.Status == status &&
                   !p.IsDeleted)
            .SumAsync(p => p.Amount);
    }
}
