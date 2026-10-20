using Microsoft.EntityFrameworkCore;
using TaskManager.Data.Context;
using TaskManager.Data.Entities;
using TaskManager.Data.Repositories.Interfaces;

namespace TaskManager.Data.Repositories
{
    public class TaskItemRepositoryEfCore : ITaskItemRepository
    {
        private readonly AppDbContext _context;

        public TaskItemRepositoryEfCore(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskItem>> GetAllAsync()
        {
            return await _context.TaskItems
                .AsNoTracking()// This method is used to improve performance when you don't need to track changes to the entities being retrieved. It tells Entity Framework Core not to track the entities in the returned result set, which can reduce memory usage and improve query performance.
                .Include(t => t.User)// This method is used to include related entities in the query results. In this case, it includes the User entity associated with each TaskItem. This is useful when you want to retrieve related data in a single query, rather than making separate queries for each related entity.
                .Include(t => t.Category)
                .ToListAsync();
        }
        /*public async Task<IEnumerable<TaskItem>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
{
    return await _context.TaskItems
        .AsNoTracking()
        .Include(t => t.User)
        .Include(t => t.Category)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
}*/
        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            return await _context.TaskItems
                .Include(t => t.User)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TaskItem> CreateAsync(TaskItem entity)
        {
            _context.TaskItems.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(TaskItem entity)
        {
            _context.TaskItems.Update(entity);
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.TaskItems.FindAsync(id);
            if (entity == null) return false;

            _context.TaskItems.Remove(entity);
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }
    }
}