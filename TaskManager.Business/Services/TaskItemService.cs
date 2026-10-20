using TaskManager.Data.Entities;
using TaskManager.Data.Repositories.Interfaces;
using TaskManager.Business.Services.Interfaces;

namespace TaskManager.Business.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly ITaskItemRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;

        public TaskItemService(
            ITaskItemRepository taskRepository,
            IUserRepository userRepository,
            ICategoryRepository categoryRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<List<TaskItem>> GetAllAsync() => await _taskRepository.GetAllAsync();

        public async Task<TaskItem?> GetByIdAsync(int id) => await _taskRepository.GetByIdAsync(id);

        public async Task<TaskItem> CreateAsync(string title, string? description, DateTime? dueDate, int userId, int categoryId)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required.");

            var userExists = await _userRepository.GetByIdAsync(userId);
            if (userExists == null)
                throw new ArgumentException($"User with id {userId} does not exist.");

            var categoryExists = await _categoryRepository.GetByIdAsync(categoryId);
            if (categoryExists == null)
                throw new ArgumentException($"Category with id {categoryId} does not exist.");

            var task = new TaskItem
            {
                Title = title.Trim(),
                Description = description?.Trim(),
                DueDate = dueDate,
                UserId = userId,
                CategoryId = categoryId
            };
            return await _taskRepository.CreateAsync(task);
        }

        public async Task<bool> UpdateAsync(int id, string title, string? description, bool isCompleted, DateTime? dueDate)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required.");

            var existing = await _taskRepository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.Title = title.Trim();
            existing.Description = description?.Trim();
            existing.IsCompleted = isCompleted;
            existing.DueDate = dueDate;
            return await _taskRepository.UpdateAsync(existing);
        }

        public async Task<bool> DeleteAsync(int id) => await _taskRepository.DeleteAsync(id);
    }
}