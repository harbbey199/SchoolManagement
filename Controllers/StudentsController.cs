using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.DTOs.Request;
using SchoolManagement.DTOs.Response;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    /// <summary>
    /// Get all students with pagination
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedResponse<List<StudentResponse>>>> GetStudents(
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10)
    {
        var students = await _studentService.GetStudentsPagedAsync(pageNumber, pageSize);
        var total = await GetTotalStudents();

        return Ok(PagedResponse<List<StudentResponse>>.SuccessResponse(
            students, total, pageNumber, pageSize, "Students retrieved"));
    }

    /// <summary>
    /// Get student by ID with detailed info
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<StudentDetailResponse>>> GetStudentDetail(int id)
    {
        var student = await _studentService.GetStudentDetailAsync(id);
        if (student == null)
            return NotFound(ApiResponse<StudentDetailResponse>.ErrorResponse("Student not found"));

        return Ok(ApiResponse<StudentDetailResponse>.SuccessResponse(student));
    }

    /// <summary>
    /// Get students by grade
    /// </summary>
    [HttpGet("grade/{grade}")]
    public async Task<ActionResult<ApiResponse<List<StudentResponse>>>> GetStudentsByGrade(string grade)
    {
        var students = await _studentService.GetStudentsByGradeAsync(grade);
        return Ok(ApiResponse<List<StudentResponse>>.SuccessResponse(students));
    }

    /// <summary>
    /// Create a new student
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<StudentResponse>>> CreateStudent([FromBody] CreateStudentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<StudentResponse>.ErrorResponse("Invalid input"));

        var student = await _studentService.CreateStudentAsync(request);
        if (student == null)
            return BadRequest(ApiResponse<StudentResponse>.ErrorResponse("Failed to create student"));

        return CreatedAtAction(nameof(GetStudentDetail), new { id = student.Id }, 
            ApiResponse<StudentResponse>.SuccessResponse(student, "Student created successfully"));
    }

    /// <summary>
    /// Update student information
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<StudentResponse>>> UpdateStudent(
        int id, [FromBody] UpdateStudentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<StudentResponse>.ErrorResponse("Invalid input"));

        var student = await _studentService.UpdateStudentAsync(id, request);
        if (student == null)
            return NotFound(ApiResponse<StudentResponse>.ErrorResponse("Student not found"));

        return Ok(ApiResponse<StudentResponse>.SuccessResponse(student, "Student updated successfully"));
    }

    /// <summary>
    /// Delete student (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteStudent(int id)
    {
        var result = await _studentService.DeleteStudentAsync(id);
        if (!result)
            return NotFound(ApiResponse<object>.ErrorResponse("Student not found"));

        return Ok(ApiResponse<object>.SuccessResponse(null, "Student deleted successfully"));
    }

    /// <summary>
    /// Assign parent to student
    /// </summary>
    [HttpPost("{studentId}/parents")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<object>>> AssignParent(
        int studentId, [FromBody] AssignParentRequest request)
    {
        var result = await _studentService.AssignParentAsync(studentId, request);
        if (!result)
            return BadRequest(ApiResponse<object>.ErrorResponse("Failed to assign parent"));

        return Ok(ApiResponse<object>.SuccessResponse(null, "Parent assigned successfully"));
    }

    private async Task<int> GetTotalStudents()
    {
        var allStudents = await _studentService.GetAllStudentsAsync();
        return allStudents.Count;
    }
}
