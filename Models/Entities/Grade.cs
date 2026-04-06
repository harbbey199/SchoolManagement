namespace SchoolManagement.Models.Entities;

public class Grade : BaseEntity
{
    public int StudentId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public decimal Marks { get; set; }
    public decimal MaxMarks { get; set; } = 100;
    public string? GradeValue { get; set; }
    public DateTime EvaluationDate { get; set; }
    public string Term { get; set; } = string.Empty;

    // Navigation properties
    public virtual Student? Student { get; set; }
}
