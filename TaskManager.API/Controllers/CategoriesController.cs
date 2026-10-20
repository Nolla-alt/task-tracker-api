using Microsoft.AspNetCore.Mvc;
using TaskManager.API.DTOs;
using TaskManager.Business.Services.Interfaces;
using TaskManager.Data.Entities;
 using Microsoft.AspNetCore.Authorization;
namespace TaskManager.API.Controllers
{
   

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
 
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        private static CategoryDto ToDto(Category category) => new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            CreatedAt = category.CreatedAt
        };

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _service.GetAllAsync();
            return Ok(categories.Select(ToDto));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _service.GetByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(ToDto(category));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            var created = await _service.CreateAsync(dto.Name);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, ToDto(created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateCategoryDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto.Name);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}