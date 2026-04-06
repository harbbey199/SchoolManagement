using SchoolManagement.Models.Entities;

namespace SchoolManagement.Repositories.Interfaces;

public interface IStudentParentRepository : IRepository<StudentParent>
{
    Task<List<StudentParent>> GetByStudentIdAsync(int studentId);
    Task<List<StudentParent>> GetByParentIdAsync(int parentId);
    Task<StudentParent?> GetByStudentAndParentAsync(int studentId, int parentId);
}
