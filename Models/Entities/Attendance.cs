namespace SchoolManagement.Models.Entities;

public enum AttendanceStatus
{
    Present = 0,
    Absent = 1,
    Late = 2,
    Excused = 3
}

public class Attendance : BaseEntity
{
    public int StudentId { get; set; }
    public DateTime AttendanceDate { get; set; }
    public AttendanceStatus Status { get; set; }
    public string? Remarks { get; set; }

    // Navigation properties
    public virtual Student? Student { get; set; }
}
