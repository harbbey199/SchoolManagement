using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.DTOs.Request;
using SchoolManagement.DTOs.Response;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GradesController : ControllerBase
{
    private readonly IGradeService _gradeService;

    public GradesController(IGradeService gradeService)
    {
        _gradeService = gradeService;
    }

    /// <summary>
    /// Record a grade for a student
    /// </summary>
    [HttpPost("record")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<ApiResponse<GradeResponse>>> RecordGrade(
        [FromBody] RecordGradeRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<GradeResponse>.ErrorResponse("Invalid input"));

        var result = await _gradeService.RecordGradeAsync(request);
        if (result == null)
            return BadRequest(ApiResponse<GradeResponse>.ErrorResponse("Failed to record grade"));

        return Ok(ApiResponse<GradeResponse>.SuccessResponse(result, "Grade recorded successfully"));
    }

    /// <summary>
    /// Get all grades for a student
    /// </summary>
    [HttpGet("student/{studentId}")]
    public async Task<ActionResult<ApiResponse<List<GradeResponse>>>> GetStudentGrades(int studentId)
    {
        var grades = await _gradeService.GetStudentGradesAsync(studentId);
        return Ok(ApiResponse<List<GradeResponse>>.SuccessResponse(grades));
    }

    /// <summary>
    /// Get grades for a student by term
    /// </summary>
    [HttpGet("student/{studentId}/term/{term}")]
    public async Task<ActionResult<ApiResponse<List<GradeResponse>>>> GetStudentGradesByTerm(
        int studentId, string term)
    {
        var grades = await _gradeService.GetStudentGradesByTermAsync(studentId, term);
        return Ok(ApiResponse<List<GradeResponse>>.SuccessResponse(grades));
    }

    /// <summary>
    /// Update a grade
    /// </summary>
    [HttpPut("{gradeId}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<ApiResponse<object>>> UpdateGrade(
        int gradeId, 
        [FromBody] RecordGradeRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.ErrorResponse("Invalid input"));

        var result = await _gradeService.UpdateGradeAsync(gradeId, request);
        if (!result)
            return NotFound(ApiResponse<object>.ErrorResponse("Grade not found"));

        return Ok(ApiResponse<object>.SuccessResponse(null, "Grade updated successfully"));
    }
}
