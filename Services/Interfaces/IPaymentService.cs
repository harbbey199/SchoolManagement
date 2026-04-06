using SchoolManagement.DTOs.Request;
using SchoolManagement.DTOs.Response;

namespace SchoolManagement.Services.Interfaces;

public interface IPaymentService
{
    Task<PaymentResponse?> RecordPaymentAsync(RecordPaymentRequest request);
    Task<List<PaymentResponse>> GetStudentPaymentsAsync(int studentId);
    Task<PaymentHistoryResponse?> GetPaymentHistoryAsync(int studentId);
    Task<bool> UpdatePaymentStatusAsync(int paymentId, int status);
}
