namespace SchoolManagement.Models.Entities;

public enum PaymentStatus
{
    Pending = 0,
    Completed = 1,
    Failed = 2,
    Refunded = 3
}

public class Payment : BaseEntity
{
    public int StudentId { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime PaymentDate { get; set; }
    public string? TransactionId { get; set; }

    // Navigation properties
    public virtual Student? Student { get; set; }
}
