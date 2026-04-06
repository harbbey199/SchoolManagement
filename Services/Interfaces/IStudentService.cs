using SchoolManagement.DTOs.Request;
using SchoolManagement.DTOs.Response;

namespace SchoolManagement.Services.Interfaces;

public interface IStudentService
{
    Task<StudentResponse?> GetStudentByIdAsync(int id);
    Task<StudentDetailResponse?> GetStudentDetailAsync(int id);
    Task<List<StudentResponse>> GetAllStudentsAsync();
    Task<List<StudentResponse>> GetStudentsByGradeAsync(string grade);
    Task<List<StudentResponse>> GetStudentsPagedAsync(int pageNumber, int pageSize);
    Task<StudentResponse?> CreateStudentAsync(CreateStudentRequest request);
    Task<StudentResponse?> UpdateStudentAsync(int id, UpdateStudentRequest request);
    Task<bool> DeleteStudentAsync(int id);
    Task<bool> AssignParentAsync(int studentId, AssignParentRequest request);
}
