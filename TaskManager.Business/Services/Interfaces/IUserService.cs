using TaskManager.Data.Entities;

namespace TaskManager.Business.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User> CreateAsync(string name, string email,string password);
        Task<bool> UpdateAsync(int id, string name, string email);
        Task<bool> DeleteAsync(int id);
    }
}