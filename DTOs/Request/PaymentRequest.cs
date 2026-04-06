namespace SchoolManagement.DTOs.Request;

public class RecordPaymentRequest
{
    public int StudentId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? TransactionId { get; set; }
}
