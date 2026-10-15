using Microsoft.AspNetCore.Mvc;
using TaskManager.API.DTOs;
using TaskManager.Business.Services.Interfaces;
using TaskManager.Data.Entities;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskItemsController : ControllerBase
    {
        private readonly ITaskItemService _service;

        public TaskItemsController(ITaskItemService service)
        {
            _service = service;
        }

        private static TaskItemDto ToDto(TaskItem task) => new TaskItemDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            UserId = task.UserId,
            CategoryId = task.CategoryId
        };

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _service.GetAllAsync();
            return Ok(tasks.Select(ToDto));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _service.GetByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(ToDto(task));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskItemDto dto)
        {
            var created = await _service.CreateAsync(
                dto.Title, dto.Description, dto.DueDate, dto.UserId, dto.CategoryId);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, ToDto(created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskItemDto dto)
        {
            var updated = await _service.UpdateAsync(
                id, dto.Title, dto.Description, dto.IsCompleted, dto.DueDate);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();// no content is returned with a 204 No Content status code to indicate that the deletion was successful
        }
    }
}