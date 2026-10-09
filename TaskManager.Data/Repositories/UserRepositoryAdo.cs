using Microsoft.Data.SqlClient;
using TaskManager.Data.Context;
using TaskManager.Data.Entities;
using TaskManager.Data.Repositories.Interfaces;

namespace TaskManager.Data.Repositories
{
    public class UserRepositoryAdo : IUserRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public UserRepositoryAdo(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<User>> GetAllAsync()
        {
            var users = new List<User>();

            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var command = new SqlCommand("SELECT Id, Name, Email, PasswordHash, CreatedAt FROM Users", connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add(new User
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2),
                    PasswordHash = reader.GetString(3),
                    CreatedAt = reader.GetDateTime(4)
                });
            }

            return users;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var command = new SqlCommand("SELECT Id, Name, Email, PasswordHash, CreatedAt FROM Users WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2),
                    PasswordHash = reader.GetString(3),
                    CreatedAt = reader.GetDateTime(4)
                };
            }

            return null;
        }

        public async Task<User> CreateAsync(User entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var command = new SqlCommand(
                "INSERT INTO Users (Name, Email, PasswordHash, CreatedAt) OUTPUT INSERTED.Id VALUES (@Name, @Email, @PasswordHash, @CreatedAt)", connection);
            command.Parameters.AddWithValue("@Name", entity.Name);
            command.Parameters.AddWithValue("@Email", entity.Email);
            command.Parameters.AddWithValue("@PasswordHash", entity.PasswordHash);
            command.Parameters.AddWithValue("@CreatedAt", entity.CreatedAt);

            var result = await command.ExecuteScalarAsync();
            entity.Id = Convert.ToInt32(result);
            return entity;
        }

        public async Task<bool> UpdateAsync(User entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var command = new SqlCommand(
                "UPDATE Users SET Name = @Name, Email = @Email, PasswordHash = @PasswordHash WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Name", entity.Name);
            command.Parameters.AddWithValue("@Email", entity.Email);
            command.Parameters.AddWithValue("@PasswordHash", entity.PasswordHash);
            command.Parameters.AddWithValue("@Id", entity.Id);

            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var command = new SqlCommand("DELETE FROM Users WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
    }
}