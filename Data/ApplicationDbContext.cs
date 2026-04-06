using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models.Entities;

namespace SchoolManagement.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        var conn = Database.GetDbConnection();
        Console.WriteLine($"EF is using: {conn.ConnectionString}");
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<Parent> Parents { get; set; } = null!;
    public DbSet<StudentParent> StudentParents { get; set; } = null!;
    public DbSet<Attendance> Attendances { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;
    public DbSet<Grade> Grades { get; set; } = null!;
    public DbSet<Report> Reports { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Student configuration
        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RollNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Grade).IsRequired().HasMaxLength(10);
            entity.Property(e => e.Section).IsRequired().HasMaxLength(10);
            entity.HasOne(e => e.User)
                .WithMany(u => u.StudentsAsStudent)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => e.RollNumber).IsUnique();
        });

        // Parent configuration
        modelBuilder.Entity<Parent>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Occupation).IsRequired().HasMaxLength(100);
            entity.HasOne(e => e.User)
                .WithMany(u => u.ParentsAsUser)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // StudentParent configuration
        modelBuilder.Entity<StudentParent>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Relationship).IsRequired().HasMaxLength(50);
            entity.HasOne(e => e.Student)
                .WithMany(s => s.StudentParents)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Parent)
                .WithMany(p => p.StudentParents)
                .HasForeignKey(e => e.ParentId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => new { e.StudentId, e.ParentId }).IsUnique();
        });

        // Attendance configuration
        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Student)
                .WithMany(s => s.Attendances)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => new { e.StudentId, e.AttendanceDate }).IsUnique();
        });

        // Payment configuration
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.HasOne(e => e.Student)
                .WithMany(s => s.Payments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Grade configuration
        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Marks).HasPrecision(5, 2);
            entity.Property(e => e.MaxMarks).HasPrecision(5, 2);
            entity.HasOne(e => e.Student)
                .WithMany(s => s.Grades)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Report configuration
        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AttendancePercentage).HasPrecision(5, 2);
            entity.Property(e => e.AverageGrade).HasPrecision(5, 2);
            entity.HasOne(e => e.Student)
                .WithMany(s => s.Reports)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
