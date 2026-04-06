namespace SchoolManagement.DTOs.Request;

public class RecordGradeRequest
{
    public int StudentId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public decimal Marks { get; set; }
    public decimal MaxMarks { get; set; } = 100;
    public string Term { get; set; } = string.Empty;
}
