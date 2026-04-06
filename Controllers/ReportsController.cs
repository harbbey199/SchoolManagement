using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.DTOs.Response;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    /// <summary>
    /// Generate a student performance report
    /// </summary>
    [HttpPost("generate")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<ApiResponse<ReportResponse>>> GenerateReport(
        [FromQuery] int studentId, 
        [FromQuery] string term)
    {
        if (string.IsNullOrWhiteSpace(term))
            return BadRequest(ApiResponse<ReportResponse>.ErrorResponse("Term is required"));

        var report = await _reportService.GenerateStudentReportAsync(studentId, term);
        if (report == null)
            return NotFound(ApiResponse<ReportResponse>.ErrorResponse("Student not found"));

        return Ok(ApiResponse<ReportResponse>.SuccessResponse(report, "Report generated successfully"));
    }

    /// <summary>
    /// Get all reports for a student
    /// </summary>
    [HttpGet("student/{studentId}")]
    public async Task<ActionResult<ApiResponse<List<ReportResponse>>>> GetStudentReports(int studentId)
    {
        var reports = await _reportService.GetStudentReportsAsync(studentId);
        return Ok(ApiResponse<List<ReportResponse>>.SuccessResponse(reports));
    }

    /// <summary>
    /// Get a specific report
    /// </summary>
    [HttpGet("{reportId}")]
    public async Task<ActionResult<ApiResponse<ReportResponse>>> GetReport(int reportId)
    {
        var report = await _reportService.GetReportAsync(reportId);
        if (report == null)
            return NotFound(ApiResponse<ReportResponse>.ErrorResponse("Report not found"));

        return Ok(ApiResponse<ReportResponse>.SuccessResponse(report));
    }
}
