using SchoolManagement.DTOs.Response;

namespace SchoolManagement.Services.Interfaces;

public interface IReportService
{
    Task<ReportResponse?> GenerateStudentReportAsync(int studentId, string term);
    Task<List<ReportResponse>> GetStudentReportsAsync(int studentId);
    Task<ReportResponse?> GetReportAsync(int reportId);
}
