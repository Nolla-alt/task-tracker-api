using TaskManager.Data.Entities;

namespace TaskManager.Business.Services.Interfaces
{
    public interface ITaskItemService
    {
        Task<List<TaskItem>> GetAllAsync();
        Task<TaskItem?> GetByIdAsync(int id);
        Task<TaskItem> CreateAsync(string title, string? description, DateTime? dueDate, int userId, int categoryId);
        Task<bool> UpdateAsync(int id, string title, string? description, bool isCompleted, DateTime? dueDate);
        Task<bool> DeleteAsync(int id);
    }
}