using Microsoft.AspNetCore.Mvc;
using MarketplaceApp.Api.Models;
using MarketplaceApp.Api.Repositories;
using System.ComponentModel.DataAnnotations;

namespace MarketplaceApp.Api.Controllers;

// 1. Simple DTO for creating (Put this in your DTOs folder later if you prefer)
public class CreateCategoryDTO
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

[Route("api/[controller]")] // URL: localhost:5040/api/Category
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly CategoryRepository _repo;

    // 2. Inject the Repository
    public CategoryController(CategoryRepository repo)
    {
        _repo = repo;
    }

    // GET: api/Category
    // Used for the "Select Category" dropdown in your Angular app
    [HttpGet]
    public IActionResult GetAll()
    {
        var categories = _repo.GetAll();
        return Ok(categories);
    }

    // GET: api/Category/5
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var category = _repo.GetById(id);
        if (category == null)
        {
            return NotFound("Category not found.");
        }
        return Ok(category);
    }

    // POST: api/Category
    // Used to add new categories (e.g., "Electronics", "Furniture")
    [HttpPost]
    public IActionResult Create([FromBody] CreateCategoryDTO dto)
    {
        // Map DTO -> Entity
        var newCategory = new Category
        {
            Name = dto.Name,
            Description = dto.Description
        };

        _repo.Create(newCategory);

        return CreatedAtAction(nameof(GetById), new { id = newCategory.CategoryId }, newCategory);
    }

    // DELETE: api/Category/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        // Check if exists first
        var existing = _repo.GetById(id);
        if (existing == null) return NotFound();

        // Note: If this category has Announcements, the database might block 
        // this delete because of the "Restrict" rule we added earlier.
        try 
        {
            _repo.Delete(id);
            return NoContent(); // Standard 204 response for delete
        }
        catch (Exception ex)
        {
            // Return a helpful error if they try to delete a category that has items
            return BadRequest("Cannot delete this category. It might have active announcements linked to it.");
        }
    }
}