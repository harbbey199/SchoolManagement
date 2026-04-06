namespace SchoolManagement.Models.Entities;

public enum UserRole
{
    Admin = 0,
    Parent = 1,
    Student = 2,
    Teacher = 3
}

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<Student>? StudentsAsStudent { get; set; }
    public virtual ICollection<Parent>? ParentsAsUser { get; set; }
}
