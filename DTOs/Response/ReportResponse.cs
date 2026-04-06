namespace SchoolManagement.DTOs.Response;

public class ReportResponse
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int TotalAttendance { get; set; }
    public int PresentDays { get; set; }
    public int AbsentDays { get; set; }
    public decimal AttendancePercentage { get; set; }
    public decimal AverageGrade { get; set; }
    public string Term { get; set; } = string.Empty;
    public DateTime GeneratedDate { get; set; }
    public string? Comments { get; set; }
    public List<GradeResponse> Grades { get; set; } = new();
}

public class ParentResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Relationship { get; set; } = string.Empty;
}
