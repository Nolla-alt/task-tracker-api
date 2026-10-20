using Microsoft.EntityFrameworkCore;
using TaskManager.Data.Context;
using TaskManager.Data.Entities;
using TaskManager.Data.Repositories.Interfaces;

namespace TaskManager.Data.Repositories
{
    public class CategoryRepositoryEfCore : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepositoryEfCore(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category> CreateAsync(Category entity)
        {
            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(Category entity)
        {
            _context.Categories.Update(entity);
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Categories.FindAsync(id);
            if (entity == null) return false;

            _context.Categories.Remove(entity);
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0; // Return true if at least one row was affected..
        }
    }
}