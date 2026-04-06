using SchoolManagement.Models.Entities;

namespace SchoolManagement.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<List<User>> GetByRoleAsync(UserRole role);
}
