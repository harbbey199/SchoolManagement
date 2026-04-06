namespace SchoolManagement.DTOs.Response;

public class GradeResponse
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public decimal Marks { get; set; }
    public decimal MaxMarks { get; set; }
    public string? GradeValue { get; set; }
    public DateTime EvaluationDate { get; set; }
    public string Term { get; set; } = string.Empty;
}
