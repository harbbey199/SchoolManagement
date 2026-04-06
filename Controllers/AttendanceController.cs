using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.DTOs.Request;
using SchoolManagement.DTOs.Response;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _attendanceService;

    public AttendanceController(IAttendanceService attendanceService)
    {
        _attendanceService = attendanceService;
    }

    /// <summary>
    /// Mark attendance for a student
    /// </summary>
    [HttpPost("mark")]
    [AllowAnonymous]
    //[Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<ApiResponse<AttendanceResponse>>> MarkAttendance(
        [FromBody] MarkAttendanceRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<AttendanceResponse>.ErrorResponse("Invalid input"));

        var result = await _attendanceService.MarkAttendanceAsync(request);
        if (result == null)
            return BadRequest(ApiResponse<AttendanceResponse>.ErrorResponse("Failed to mark attendance"));

        return Ok(ApiResponse<AttendanceResponse>.SuccessResponse(result, "Attendance marked successfully"));
    }

    /// <summary>
    /// Mark attendance for multiple students
    /// </summary>
    [HttpPost("mark-multiple")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<ApiResponse<object>>> MarkMultipleAttendance(
        [FromBody] MarkMultipleAttendanceRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.ErrorResponse("Invalid input"));

        var result = await _attendanceService.MarkMultipleAttendanceAsync(request);
        if (!result)
            return BadRequest(ApiResponse<object>.ErrorResponse("Failed to mark attendance"));

        return Ok(ApiResponse<object>.SuccessResponse(null, "Attendance marked successfully"));
    }

    /// <summary>
    /// Get attendance records for a student
    /// </summary>
    [HttpGet("student/{studentId}")]
    public async Task<ActionResult<ApiResponse<List<AttendanceResponse>>>> GetStudentAttendance(int studentId)
    {
        var records = await _attendanceService.GetStudentAttendanceAsync(studentId);
        return Ok(ApiResponse<List<AttendanceResponse>>.SuccessResponse(records));
    }

    /// <summary>
    /// Get attendance by date range
    /// </summary>
    [HttpGet("student/{studentId}/range")]
    public async Task<ActionResult<ApiResponse<List<AttendanceResponse>>>> GetAttendanceByDateRange(
        int studentId, 
        [FromQuery] DateTime startDate, 
        [FromQuery] DateTime endDate)
    {
        var records = await _attendanceService.GetAttendanceByDateRangeAsync(
            studentId, startDate, endDate);
        return Ok(ApiResponse<List<AttendanceResponse>>.SuccessResponse(records));
    }

    /// <summary>
    /// Get attendance summary for a student
    /// </summary>
    [HttpGet("student/{studentId}/summary")]
    public async Task<ActionResult<ApiResponse<AttendanceSummaryResponse>>> GetAttendanceSummary(
        int studentId,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        startDate ??= DateTime.UtcNow.AddMonths(-3);
        endDate ??= DateTime.UtcNow;

        var summary = await _attendanceService.GetAttendanceSummaryAsync(
            studentId, startDate.Value, endDate.Value);

        if (summary == null)
            return NotFound(ApiResponse<AttendanceSummaryResponse>.ErrorResponse("Student not found"));

        return Ok(ApiResponse<AttendanceSummaryResponse>.SuccessResponse(summary));
    }
}
