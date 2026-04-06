using SchoolManagement.Models.Entities;

namespace SchoolManagement.Repositories.Interfaces;

public interface IStudentRepository : IRepository<Student>
{
    Task<Student?> GetStudentWithDetailsAsync(int id);
    Task<List<Student>> GetStudentsByGradeAsync(string grade);
    Task<Student?> GetByRollNumberAsync(string rollNumber);
    Task<List<Student>> GetPaginatedAsync(int pageNumber, int pageSize);
}
