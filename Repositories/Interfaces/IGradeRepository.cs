using SchoolManagement.Models.Entities;

namespace SchoolManagement.Repositories.Interfaces;

public interface IGradeRepository : IRepository<Grade>
{
    Task<List<Grade>> GetByStudentIdAsync(int studentId);
    Task<List<Grade>> GetByStudentAndTermAsync(int studentId, string term);
    Task<decimal> GetAverageGradeByStudentAsync(int studentId);
}
