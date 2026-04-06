namespace SchoolManagement.DTOs.Response;

public class PaymentResponse
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime PaymentDate { get; set; }
    public string? TransactionId { get; set; }
}

public class PaymentHistoryResponse
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public List<PaymentResponse> Payments { get; set; } = new();
    public decimal TotalAmount { get; set; }
}
