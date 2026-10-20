using TaskManager.Data.Entities;
using TaskManager.Data.Repositories.Interfaces;
using TaskManager.Business.Services.Interfaces;

namespace TaskManager.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Category> CreateAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name is required.");

            var category = new Category { Name = name.Trim() };
            return await _repository.CreateAsync(category);
        }

        public async Task<bool> UpdateAsync(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name is required.");

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.Name = name.Trim();
            return await _repository.UpdateAsync(existing);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}