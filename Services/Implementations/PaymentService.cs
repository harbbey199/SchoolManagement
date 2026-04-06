using SchoolManagement.DTOs.Request;
using SchoolManagement.DTOs.Response;
using SchoolManagement.Models.Entities;
using SchoolManagement.Repositories.Interfaces;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Services.Implementations;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IUserRepository _userRepository;

    public PaymentService(
        IPaymentRepository paymentRepository,
        IStudentRepository studentRepository,
        IUserRepository userRepository)
    {
        _paymentRepository = paymentRepository;
        _studentRepository = studentRepository;
        _userRepository = userRepository;
    }

    public async Task<PaymentResponse?> RecordPaymentAsync(RecordPaymentRequest request)
    {
        var student = await _studentRepository.GetByIdAsync(request.StudentId);
        if (student == null)
            return null;

        var payment = new Payment
        {
            StudentId = request.StudentId,
            Amount = request.Amount,
            Description = request.Description,
            Status = PaymentStatus.Completed,
            PaymentDate = DateTime.UtcNow,
            TransactionId = request.TransactionId
        };

        var createdPayment = await _paymentRepository.AddAsync(payment);
        return await MapToPaymentResponseAsync(createdPayment);
    }

    public async Task<List<PaymentResponse>> GetStudentPaymentsAsync(int studentId)
    {
        var payments = await _paymentRepository.GetByStudentIdAsync(studentId);
        var responses = new List<PaymentResponse>();

        foreach (var payment in payments)
        {
            var response = await MapToPaymentResponseAsync(payment);
            if (response != null)
                responses.Add(response);
        }

        return responses;
    }

    public async Task<PaymentHistoryResponse?> GetPaymentHistoryAsync(int studentId)
    {
        var student = await _studentRepository.GetByIdAsync(studentId);
        if (student == null)
            return null;

        var user = await _userRepository.GetByIdAsync(student.UserId);
        var payments = await GetStudentPaymentsAsync(studentId);
        var totalAmount = payments.Sum(p => p.Amount);

        return new PaymentHistoryResponse
        {
            StudentId = studentId,
            StudentName = $"{user!.FirstName} {user.LastName}",
            Payments = payments,
            TotalAmount = totalAmount
        };
    }

    public async Task<bool> UpdatePaymentStatusAsync(int paymentId, int status)
    {
        var payment = await _paymentRepository.GetByIdAsync(paymentId);
        if (payment == null)
            return false;

        payment.Status = (PaymentStatus)status;
        payment.UpdatedAt = DateTime.UtcNow;
        await _paymentRepository.UpdateAsync(payment);
        return true;
    }

    private async Task<PaymentResponse?> MapToPaymentResponseAsync(Payment payment)
    {
        var student = await _studentRepository.GetByIdAsync(payment.StudentId);
        var user = student != null ? await _userRepository.GetByIdAsync(student.UserId) : null;

        if (user == null)
            return null;

        return new PaymentResponse
        {
            Id = payment.Id,
            StudentId = payment.StudentId,
            StudentName = $"{user.FirstName} {user.LastName}",
            Amount = payment.Amount,
            Status = payment.Status.ToString(),
            Description = payment.Description,
            PaymentDate = payment.PaymentDate,
            TransactionId = payment.TransactionId
        };
    }
}
