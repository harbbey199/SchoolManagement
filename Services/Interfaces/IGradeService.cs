using SchoolManagement.DTOs.Request;
using SchoolManagement.DTOs.Response;

namespace SchoolManagement.Services.Interfaces;

public interface IGradeService
{
    Task<GradeResponse?> RecordGradeAsync(RecordGradeRequest request);
    Task<List<GradeResponse>> GetStudentGradesAsync(int studentId);
    Task<List<GradeResponse>> GetStudentGradesByTermAsync(int studentId, string term);
    Task<bool> UpdateGradeAsync(int gradeId, RecordGradeRequest request);
}
