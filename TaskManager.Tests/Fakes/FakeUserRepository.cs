using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Entities;
using TaskManager.Data.Repositories.Interfaces;
using TaskManager.Data.Context;




public class FakeUserRepository : IUserRepository
{
    public List<User> Users { get; } = new();

    public Task<List<User>> GetAllAsync() => Task.FromResult(Users.ToList());
    public Task<User?> GetByIdAsync(int id) => Task.FromResult(Users.FirstOrDefault(u => u.Id == id));
    public Task<User?> GetByEmailAsync(string email) => Task.FromResult(Users.FirstOrDefault(u => u.Email == email));
    public Task<User> CreateAsync(User user) { user.Id = Users.Count + 1; Users.Add(user); return Task.FromResult(user); }
    public Task<bool> UpdateAsync(User user) => Task.FromResult(true);
    public Task<bool> DeleteAsync(int id) { Users.RemoveAll(u => u.Id == id); return Task.FromResult(true); }
}
/*Task<List<User>> GetAllAsync();
Task<User?> GetByIdAsync(int id);
Task<User?> GetByEmailAsync(string email);
Task<User> CreateAsync(User entity);
Task<bool> UpdateAsync(User entity);
Task<bool> DeleteAsync(int id);*/