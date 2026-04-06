namespace SchoolManagement.Models.Entities;

public class Parent : BaseEntity
{
    public int UserId { get; set; }
    public string Occupation { get; set; } = string.Empty;
    public string? Address { get; set; }

    // Navigation properties
    public virtual User? User { get; set; }
    public virtual ICollection<StudentParent> StudentParents { get; set; } = new List<StudentParent>();
}
