using Microsoft.AspNetCore.Mvc;
using MarketplaceApp.Api.Models;
using MarketplaceApp.Api.Repositories;
using System.ComponentModel.DataAnnotations;
using MarketplaceApp.Api.Models.AnnouncementDTOs;

namespace MarketplaceApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
// FIX 1: Inherit from ControllerBase so you can use Ok(), NotFound(), etc.
public class AnnouncementController : ControllerBase 
{
    // FIX 2: Use the Interface (I...), not the concrete class
    private readonly AnnouncementsRepository _repo;

    public AnnouncementController(AnnouncementsRepository repo)
    {
        _repo = repo;
    }

    // GET: api/Announcement
    [HttpGet]
    public IActionResult GetAll()
    {
        var items = _repo.GetAll();

        var response = items.Select(a => new AnnouncementResponseDTO
        {
            Id = a.AnnouncementId,
            Title = a.Title,
            Description = a.Description,
            Price = a.Price,
            ImageUrl = a.ImageUrl,
            PostedDate = a.PostedDate,
            CategoryName = a.Category?.Name ?? "Unknown",
            SellerId = a.UserId,
            SellerName = $"{a.User?.FirstName} {a.User?.LastName}",
            SellerAvatar = a.User?.UserProfile?.Pfp
        });

        return Ok(response);
    }

    [HttpGet("{name}")]
    public IActionResult GetByName(string name)
    {
        return Ok(_repo.SearchByTitle(name));
    }

    // GET: api/Announcement/5
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var a = _repo.GetById(id);
        
        // FIX 3: Added simple NotFound check
        if (a == null) return NotFound("Announcement not found");

        var dto = new AnnouncementResponseDTO
        {
            Id = a.AnnouncementId,
            Title = a.Title,
            Description = a.Description,
            Price = a.Price,
            ImageUrl = a.ImageUrl,
            PostedDate = a.PostedDate,
            CategoryName = a.Category?.Name ?? "Unknown",
            SellerId = a.UserId,
            SellerName = $"{a.User?.FirstName} {a.User?.LastName}",
            SellerAvatar = a.User?.UserProfile?.Pfp
        };

        return Ok(dto);
    }

    // POST: api/Announcement
    [HttpPost]
    public IActionResult Create([FromBody] CreateAnnouncementDTO dto)
    {
        var announcement = new Announcement
        {
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price,
            CategoryId = dto.CategoryId,
            UserId = dto.UserId,
            ImageUrl = dto.ImageUrl,
            PostedDate = DateTime.UtcNow 
        };

        _repo.Create(announcement);

        // helper method creates a "201 Created" response with a Location header
        return CreatedAtAction(nameof(GetById), new { id = announcement.AnnouncementId }, new { message = "Created", id = announcement.AnnouncementId });
    }

    // DELETE: api/Announcement/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var existing = _repo.GetById(id);
        if (existing == null) return NotFound();

        _repo.Delete(id);
        return NoContent();
    }
    
    // GET: api/Announcement/category/2
    [HttpGet("category/{categoryId}")]
    public IActionResult GetByCategory(int categoryId)
    {
        var items = _repo.GetByCategoryId(categoryId);
        
        // Map to DTO (Consistency is key!)
        var response = items.Select(a => new AnnouncementResponseDTO
        {
            Id = a.AnnouncementId,
            Title = a.Title,
            Description = a.Description,
            Price = a.Price,
            ImageUrl = a.ImageUrl,
            PostedDate = a.PostedDate,
            CategoryName = a.Category?.Name ?? "Unknown",
            SellerId = a.UserId,
            SellerName = $"{a.User?.FirstName} {a.User?.LastName}",
            SellerAvatar = a.User?.UserProfile?.Pfp
        });

        return Ok(response);
    }
}