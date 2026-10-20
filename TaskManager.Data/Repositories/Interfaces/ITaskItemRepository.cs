using TaskManager.Data.Entities;

namespace TaskManager.Data.Repositories.Interfaces
{
    public interface ITaskItemRepository
    {
        Task<List<TaskItem>> GetAllAsync();
        Task<TaskItem?> GetByIdAsync(int id);
        Task<TaskItem> CreateAsync(TaskItem entity);
        Task<bool> UpdateAsync(TaskItem entity);
        Task<bool> DeleteAsync(int id);
    }
}