using Microsoft.Data.SqlClient;
using TaskManager.Data.Context;
using TaskManager.Data.Entities;
using TaskManager.Data.Repositories.Interfaces;

namespace TaskManager.Data.Repositories
{
    public class CategoryRepositoryAdo : ICategoryRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public CategoryRepositoryAdo(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            var categories = new List<Category>();

            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var command = new SqlCommand("SELECT Id, Name, CreatedAt FROM Categories", connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                categories.Add(new Category
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    CreatedAt = reader.GetDateTime(2)
                });
            }

            return categories;
        }
        public async Task<Category?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var command = new SqlCommand("SELECT Id, Name, CreatedAt FROM Categories WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Category
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    CreatedAt = reader.GetDateTime(2)
                };
            }

            return null;
        }

        public async Task<Category> CreateAsync(Category entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var command = new SqlCommand(
                "INSERT INTO Categories (Name, CreatedAt) OUTPUT INSERTED.Id VALUES (@Name, @CreatedAt)", connection);
            command.Parameters.AddWithValue("@Name", entity.Name);
            command.Parameters.AddWithValue("@CreatedAt", entity.CreatedAt);

            var result = await command.ExecuteScalarAsync();
            entity.Id = Convert.ToInt32(result);
            return entity;
        }

        public async Task<bool> UpdateAsync(Category entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var command = new SqlCommand("UPDATE Categories SET Name = @Name WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Name", entity.Name);
            command.Parameters.AddWithValue("@Id", entity.Id);

            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var command = new SqlCommand("DELETE FROM Categories WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
    }
}