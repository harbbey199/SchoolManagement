using SchoolManagement.DTOs.Response;
using SchoolManagement.Models.Entities;
using SchoolManagement.Repositories.Interfaces;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Services.Implementations;

public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGradeRepository _gradeRepository;
    private readonly IAttendanceRepository _attendanceRepository;

    public ReportService(
        IReportRepository reportRepository,
        IStudentRepository studentRepository,
        IUserRepository userRepository,
        IGradeRepository gradeRepository,
        IAttendanceRepository attendanceRepository)
    {
        _reportRepository = reportRepository;
        _studentRepository = studentRepository;
        _userRepository = userRepository;
        _gradeRepository = gradeRepository;
        _attendanceRepository = attendanceRepository;
    }

    public async Task<ReportResponse?> GenerateStudentReportAsync(int studentId, string term)
    {
        var student = await _studentRepository.GetByIdAsync(studentId);
        if (student == null)
            return null;

        var user = await _userRepository.GetByIdAsync(student.UserId);
        var grades = await _gradeRepository.GetByStudentAndTermAsync(studentId, term);
        var attendanceSummary = await _attendanceRepository.GetAttendanceSummaryAsync(
            studentId, DateTime.UtcNow.AddMonths(-3), DateTime.UtcNow);

        var averageGrade = grades.Any() 
            ? grades.Average(g => (g.Marks / g.MaxMarks) * 100) 
            : 0;

        var report = new Report
        {
            StudentId = studentId,
            TotalAttendance = attendanceSummary.TotalDays,
            PresentDays = attendanceSummary.PresentDays,
            AbsentDays = attendanceSummary.AbsentDays,
            AttendancePercentage = attendanceSummary.TotalDays > 0 
                ? (attendanceSummary.PresentDays * 100m) / attendanceSummary.TotalDays 
                : 0,
            AverageGrade = (decimal)averageGrade,
            Term = term,
            GeneratedDate = DateTime.UtcNow,
            Comments = GenerateComments((double)averageGrade, attendanceSummary.TotalDays)
        };

        var createdReport = await _reportRepository.AddAsync(report);
        return MapToReportResponse(createdReport, user, grades);
    }

    public async Task<List<ReportResponse>> GetStudentReportsAsync(int studentId)
    {
        var reports = await _reportRepository.GetByStudentAsync(studentId);
        var responses = new List<ReportResponse>();

        var student = await _studentRepository.GetByIdAsync(studentId);
        var user = student != null ? await _userRepository.GetByIdAsync(student.UserId) : null;

        foreach (var report in reports)
        {
            var grades = await _gradeRepository.GetByStudentAndTermAsync(studentId, report.Term);
            responses.Add(MapToReportResponse(report, user, grades));
        }

        return responses;
    }

    public async Task<ReportResponse?> GetReportAsync(int reportId)
    {
        var report = await _reportRepository.GetByIdAsync(reportId);
        if (report == null)
            return null;

        var student = await _studentRepository.GetByIdAsync(report.StudentId);
        var user = student != null ? await _userRepository.GetByIdAsync(student.UserId) : null;
        var grades = await _gradeRepository.GetByStudentAndTermAsync(report.StudentId, report.Term);

        return MapToReportResponse(report, user, grades);
    }

    private ReportResponse MapToReportResponse(Report report, User? user, List<Grade> grades)
    {
        var gradeResponses = grades.Select(g => new GradeResponse
        {
            Id = g.Id,
            StudentId = g.StudentId,
            StudentName = $"{user?.FirstName} {user?.LastName}",
            Subject = g.Subject,
            Marks = g.Marks,
            MaxMarks = g.MaxMarks,
            GradeValue = g.GradeValue,
            EvaluationDate = g.EvaluationDate,
            Term = g.Term
        }).ToList();

        return new ReportResponse
        {
            Id = report.Id,
            StudentId = report.StudentId,
            StudentName = $"{user?.FirstName} {user?.LastName}",
            TotalAttendance = report.TotalAttendance,
            PresentDays = report.PresentDays,
            AbsentDays = report.AbsentDays,
            AttendancePercentage = report.AttendancePercentage,
            AverageGrade = report.AverageGrade,
            Term = report.Term,
            GeneratedDate = report.GeneratedDate,
            Comments = report.Comments,
            Grades = gradeResponses
        };
    }

    private string GenerateComments(double averageGrade, int totalAttendance)
    {
        var comments = new List<string>();

        if (averageGrade >= 80)
            comments.Add("Excellent academic performance.");
        else if (averageGrade >= 70)
            comments.Add("Good academic performance.");
        else if (averageGrade >= 60)
            comments.Add("Average academic performance.");
        else
            comments.Add("Below average academic performance. Requires improvement.");

        if (totalAttendance > 0)
            comments.Add($"Total attendance: {totalAttendance} days.");

        return string.Join(" ", comments);
    }
}
