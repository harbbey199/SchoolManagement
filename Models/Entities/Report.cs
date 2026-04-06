namespace SchoolManagement.Models.Entities;

public class Report : BaseEntity
{
    public int StudentId { get; set; }
    public int TotalAttendance { get; set; }
    public int PresentDays { get; set; }
    public int AbsentDays { get; set; }
    public decimal AttendancePercentage { get; set; }
    public decimal AverageGrade { get; set; }
    public string Term { get; set; } = string.Empty;
    public DateTime GeneratedDate { get; set; } = DateTime.UtcNow;
    public string? Comments { get; set; }

    // Navigation properties
    public virtual Student? Student { get; set; }
}
