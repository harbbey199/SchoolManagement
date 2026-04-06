using SchoolManagement.Models.Entities;

namespace SchoolManagement.Repositories.Interfaces;

public interface IReportRepository : IRepository<Report>
{
    Task<Report?> GetByStudentAndTermAsync(int studentId, string term);
    Task<List<Report>> GetByStudentAsync(int studentId);
}
