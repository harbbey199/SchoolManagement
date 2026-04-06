using SchoolManagement.Models.Entities;

namespace SchoolManagement.Repositories.Interfaces;

public interface IPaymentRepository : IRepository<Payment>
{
    Task<List<Payment>> GetByStudentIdAsync(int studentId);
    Task<List<Payment>> GetByStatusAsync(PaymentStatus status);
    Task<decimal> GetTotalByStudentAsync(int studentId);
    Task<decimal> GetAmountByStudentAndStatusAsync(int studentId, PaymentStatus status);
}
