namespace SchoolManagement.DTOs.Request;

public class MarkAttendanceRequest
{
    public int StudentId { get; set; }
    public DateTime AttendanceDate { get; set; }
    public int Status { get; set; }
    public string? Remarks { get; set; }
}

public class MarkMultipleAttendanceRequest
{
    public List<AttendanceRecord> Records { get; set; } = new();
}

public class AttendanceRecord
{
    public int StudentId { get; set; }
    public int Status { get; set; }
    public string? Remarks { get; set; }
}
