using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Entities;

namespace TaskManager.Data.Repositories.Interfaces
{
    internal interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();// Get all categories
        Task<Category?> GetByIdAsync(int id);// Get category by id
        Task<Category> CreateAsync(Category category);// Create a new category
        Task<bool> UpdateAsync(Category category);// Update an existing category
        Task<bool> DeleteAsync(int id);// Delete a category by id
    }
}
