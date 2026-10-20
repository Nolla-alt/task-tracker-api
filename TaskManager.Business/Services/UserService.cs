using TaskManager.Data.Entities;
using TaskManager.Data.Repositories.Interfaces;
using TaskManager.Business.Services.Interfaces;
using BCrypt.Net;
namespace TaskManager.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<User>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<User?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<User> CreateAsync(string name, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.");
            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))// Simple email validation
                throw new ArgumentException("A valid email is required.");
            if (string.IsNullOrWhiteSpace(password)||password.Length<8)

                throw new ArgumentException("Password must be at least 8 characters.");
            var existing = await _repository.GetByEmailAsync(email.Trim().ToLower());

            if (existing != null)
                throw new ArgumentException("This email is already registered.");

            var user = new User { Name = name.Trim(), Email = email.Trim().ToLower(),PasswordHash= BCrypt.Net.BCrypt.HashPassword(password) };
            return await _repository.CreateAsync(user);
        }

        public async Task<bool> UpdateAsync(int id, string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.");
            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
                throw new ArgumentException("A valid email is required.");

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.Name = name.Trim();
            existing.Email = email.Trim().ToLower();
            return await _repository.UpdateAsync(existing);
        }

        public async Task<bool> DeleteAsync(int id) => await _repository.DeleteAsync(id);
        //Login method to verify user credentials
        public async Task<User?> LoginAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Email and password are required.");

            var user = await _repository.GetByEmailAsync(email.Trim().ToLower());
            if (user == null || string.IsNullOrEmpty(user.PasswordHash))
                return null;

            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash) ? user : null;
        }
    }

}