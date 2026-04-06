using Moq;
using Xunit;
using SchoolManagement.DTOs.Request;
using SchoolManagement.DTOs.Response;
using SchoolManagement.Models.Entities;
using SchoolManagement.Repositories.Interfaces;
using SchoolManagement.Services.Implementations;

namespace SchoolManagement.Tests;

public class StudentServiceTests
{
    private readonly Mock<IStudentRepository> _mockStudentRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IParentRepository> _mockParentRepository;
    private readonly Mock<IStudentParentRepository> _mockStudentParentRepository;
    private readonly Mock<IGradeRepository> _mockGradeRepository;
    private readonly Mock<IAttendanceRepository> _mockAttendanceRepository;
    private readonly StudentService _studentService;

    public StudentServiceTests()
    {
        _mockStudentRepository = new Mock<IStudentRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockParentRepository = new Mock<IParentRepository>();
        _mockStudentParentRepository = new Mock<IStudentParentRepository>();
        _mockGradeRepository = new Mock<IGradeRepository>();
        _mockAttendanceRepository = new Mock<IAttendanceRepository>();

        _studentService = new StudentService(
            _mockStudentRepository.Object,
            _mockUserRepository.Object,
            _mockParentRepository.Object,
            _mockStudentParentRepository.Object,
            _mockGradeRepository.Object,
            _mockAttendanceRepository.Object
        );
    }

    [Fact]
    public async Task GetStudentByIdAsync_WithValidId_ReturnsStudent()
    {
        // Arrange
        var studentId = 1;
        var user = new User
        {
            Id = 1,
            Email = "student@test.com",
            FirstName = "John",
            LastName = "Doe"
        };
        var student = new Student
        {
            Id = studentId,
            UserId = 1,
            RollNumber = "001",
            Grade = "10A",
            Section = "A"
        };

        _mockStudentRepository.Setup(r => r.GetByIdAsync(studentId))
            .ReturnsAsync(student);
        _mockUserRepository.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(user);

        // Act
        var result = await _studentService.GetStudentByIdAsync(studentId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(studentId, result.Id);
        Assert.Equal("student@test.com", result.Email);
        Assert.Equal("John", result.FirstName);
    }

    [Fact]
    public async Task GetStudentByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        _mockStudentRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Student?)null);

        // Act
        var result = await _studentService.GetStudentByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateStudentAsync_WithValidRequest_CreatesStudent()
    {
        // Arrange
        var request = new CreateStudentRequest
        {
            Email = "newstudent@test.com",
            FirstName = "Jane",
            LastName = "Smith",
            Password = "Password123!",
            RollNumber = "002",
            Grade = "10B",
            Section = "B",
            DateOfBirth = new DateTime(2008, 5, 15)
        };

        var createdUser = new User
        {
            Id = 2,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = UserRole.Student
        };

        var createdStudent = new Student
        {
            Id = 1,
            UserId = 2,
            RollNumber = request.RollNumber,
            Grade = request.Grade,
            Section = request.Section,
            DateOfBirth = request.DateOfBirth
        };

        _mockUserRepository.Setup(r => r.AddAsync(It.IsAny<User>()))
            .ReturnsAsync(createdUser);
        _mockStudentRepository.Setup(r => r.AddAsync(It.IsAny<Student>()))
            .ReturnsAsync(createdStudent);

        // Act
        var result = await _studentService.CreateStudentAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("newstudent@test.com", result.Email);
        Assert.Equal("Jane", result.FirstName);
        _mockUserRepository.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
        _mockStudentRepository.Verify(r => r.AddAsync(It.IsAny<Student>()), Times.Once);
    }

    [Fact]
    public async Task DeleteStudentAsync_WithValidId_DeletesStudent()
    {
        // Arrange
        var studentId = 1;
        var student = new Student
        {
            Id = studentId,
            IsDeleted = false
        };

        _mockStudentRepository.Setup(r => r.GetByIdAsync(studentId))
            .ReturnsAsync(student);
        _mockStudentRepository.Setup(r => r.UpdateAsync(It.IsAny<Student>()))
            .ReturnsAsync(student);

        // Act
        var result = await _studentService.DeleteStudentAsync(studentId);

        // Assert
        Assert.True(result);
        Assert.True(student.IsDeleted);
        _mockStudentRepository.Verify(r => r.UpdateAsync(It.IsAny<Student>()), Times.Once);
    }

    [Fact]
    public async Task AssignParentAsync_WithValidData_AssignsParent()
    {
        // Arrange
        var studentId = 1;
        var parentId = 1;
        var student = new Student { Id = studentId };
        var parent = new Parent { Id = parentId };
        var request = new AssignParentRequest { ParentId = parentId, Relationship = "Father" };

        _mockStudentRepository.Setup(r => r.GetByIdAsync(studentId))
            .ReturnsAsync(student);
        _mockParentRepository.Setup(r => r.GetByIdAsync(parentId))
            .ReturnsAsync(parent);
        _mockStudentParentRepository.Setup(r => r.GetByStudentAndParentAsync(studentId, parentId))
            .ReturnsAsync((StudentParent?)null);
        _mockStudentParentRepository.Setup(r => r.AddAsync(It.IsAny<StudentParent>()))
            .ReturnsAsync(new StudentParent { StudentId = studentId, ParentId = parentId });

        // Act
        var result = await _studentService.AssignParentAsync(studentId, request);

        // Assert
        Assert.True(result);
        _mockStudentParentRepository.Verify(r => r.AddAsync(It.IsAny<StudentParent>()), Times.Once);
    }
}

public class AttendanceServiceTests
{
    private readonly Mock<IAttendanceRepository> _mockAttendanceRepository;
    private readonly Mock<IStudentRepository> _mockStudentRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly AttendanceService _attendanceService;

    public AttendanceServiceTests()
    {
        _mockAttendanceRepository = new Mock<IAttendanceRepository>();
        _mockStudentRepository = new Mock<IStudentRepository>();
        _mockUserRepository = new Mock<IUserRepository>();

        _attendanceService = new AttendanceService(
            _mockAttendanceRepository.Object,
            _mockStudentRepository.Object,
            _mockUserRepository.Object
        );
    }

    [Fact]
    public async Task MarkAttendanceAsync_WithValidRequest_MarksAttendance()
    {
        // Arrange
        var request = new MarkAttendanceRequest
        {
            StudentId = 1,
            AttendanceDate = DateTime.UtcNow,
            Status = 0,
            Remarks = "Present"
        };

        var attendance = new Attendance
        {
            Id = 1,
            StudentId = request.StudentId,
            Status = (AttendanceStatus)request.Status
        };

        var student = new Student { Id = 1, UserId = 1 };
        var user = new User { FirstName = "John", LastName = "Doe" };

        _mockAttendanceRepository.Setup(r => r.GetByStudentAndDateAsync(request.StudentId, request.AttendanceDate))
            .ReturnsAsync((Attendance?)null);
        _mockAttendanceRepository.Setup(r => r.AddAsync(It.IsAny<Attendance>()))
            .ReturnsAsync(attendance);
        _mockStudentRepository.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(student);
        _mockUserRepository.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(user);

        // Act
        var result = await _attendanceService.MarkAttendanceAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.StudentId);
        _mockAttendanceRepository.Verify(r => r.AddAsync(It.IsAny<Attendance>()), Times.Once);
    }
}
