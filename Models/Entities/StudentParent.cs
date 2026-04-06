namespace SchoolManagement.Models.Entities;

public class StudentParent : BaseEntity
{
    public int StudentId { get; set; }
    public int ParentId { get; set; }
    public string Relationship { get; set; } = string.Empty;

    // Navigation properties
    public virtual Student? Student { get; set; }
    public virtual Parent? Parent { get; set; }
}
