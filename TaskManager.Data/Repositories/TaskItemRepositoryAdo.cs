using Microsoft.Data.SqlClient;
using TaskManager.Data.Context;
using TaskManager.Data.Entities;
using TaskManager.Data.Repositories.Interfaces;

namespace TaskManager.Data.Repositories
{
    public class TaskItemRepositoryAdo : ITaskItemRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public TaskItemRepositoryAdo(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        private const string BaseSelect = @"
            SELECT t.Id, t.Title, t.Description, t.IsCompleted, t.DueDate, t.CreatedAt,
                   t.UserId, u.Name AS UserName, u.Email,
                   t.CategoryId, c.Name AS CategoryName
            FROM TaskItems t
            JOIN Users u ON t.UserId = u.Id
            JOIN Categories c ON t.CategoryId = c.Id";

        private static TaskItem MapRow(SqlDataReader reader)
        {
            return new TaskItem
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                IsCompleted = reader.GetBoolean(3),
                DueDate = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                CreatedAt = reader.GetDateTime(5),
                UserId = reader.GetInt32(6),
                User = new User { Id = reader.GetInt32(6), Name = reader.GetString(7), Email = reader.GetString(8) },
                CategoryId = reader.GetInt32(9),
                Category = new Category { Id = reader.GetInt32(9), Name = reader.GetString(10) }
            };
        }

        public async Task<List<TaskItem>> GetAllAsync()
        {
            var tasks = new List<TaskItem>();

            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var command = new SqlCommand(BaseSelect, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                tasks.Add(MapRow(reader));
            }

            return tasks;
        }

        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var command = new SqlCommand(BaseSelect + " WHERE t.Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapRow(reader);
            }

            return null;
        }

        public async Task<TaskItem> CreateAsync(TaskItem entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var command = new SqlCommand(@"
                INSERT INTO TaskItems (Title, Description, IsCompleted, DueDate, CreatedAt, UserId, CategoryId)
                OUTPUT INSERTED.Id
                VALUES (@Title, @Description, @IsCompleted, @DueDate, @CreatedAt, @UserId, @CategoryId)", connection);

            command.Parameters.AddWithValue("@Title", entity.Title);
            command.Parameters.AddWithValue("@Description", (object?)entity.Description ?? DBNull.Value);
            command.Parameters.AddWithValue("@IsCompleted", entity.IsCompleted);
            command.Parameters.AddWithValue("@DueDate", (object?)entity.DueDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@CreatedAt", entity.CreatedAt);
            command.Parameters.AddWithValue("@UserId", entity.UserId);
            command.Parameters.AddWithValue("@CategoryId", entity.CategoryId);

            var result = await command.ExecuteScalarAsync();
            entity.Id = Convert.ToInt32(result);
            return entity;
        }

        public async Task<bool> UpdateAsync(TaskItem entity)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var command = new SqlCommand(@"
                UPDATE TaskItems
                SET Title = @Title, Description = @Description, IsCompleted = @IsCompleted,
                    DueDate = @DueDate, UserId = @UserId, CategoryId = @CategoryId
                WHERE Id = @Id", connection);

            command.Parameters.AddWithValue("@Title", entity.Title);
            command.Parameters.AddWithValue("@Description", (object?)entity.Description ?? DBNull.Value);
            command.Parameters.AddWithValue("@IsCompleted", entity.IsCompleted);
            command.Parameters.AddWithValue("@DueDate", (object?)entity.DueDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@UserId", entity.UserId);
            command.Parameters.AddWithValue("@CategoryId", entity.CategoryId);
            command.Parameters.AddWithValue("@Id", entity.Id);

            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var command = new SqlCommand("DELETE FROM TaskItems WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
    }
}