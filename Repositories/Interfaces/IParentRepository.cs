using SchoolManagement.Models.Entities;

namespace SchoolManagement.Repositories.Interfaces;

public interface IParentRepository : IRepository<Parent>
{
    Task<Parent?> GetByUserIdAsync(int userId);
}
