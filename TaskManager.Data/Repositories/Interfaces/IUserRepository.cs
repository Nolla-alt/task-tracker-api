using TaskManager.Data.Entities;

namespace TaskManager.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User> CreateAsync(User entity);
        Task<bool> UpdateAsync(User entity);
        Task<bool> DeleteAsync(int id);
    }
}