using Microsoft.AspNetCore.Mvc;
using MarketplaceApp.Api.Models;
using MarketplaceApp.Api.Repositories;
using System.ComponentModel.DataAnnotations;
using MarketplaceApp.Api.Models.AnnouncementDTOs;
using Microsoft.AspNetCore.Http;
using System.IO;
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
            SellerAvatar = a.User?.UserProfile?.Pfp,
            SellerPhone = a.User?.PhoneNumber
        });

        return Ok(response);
    }

    [HttpGet("{name}")]
    public IActionResult GetByName(string name)
    {
        return Ok(_repo.SearchByTitle(name));
    }

    // GET: api/Announcement/5
    [HttpGet("{id:int}")]
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
    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        // 1. Check if a file was actually sent
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        // 2. Validate file type (Optional but safe)
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var extension = Path.GetExtension(file.FileName).ToLower();
        
        if (!allowedExtensions.Contains(extension))
            return BadRequest("Invalid file type. Only JPG, PNG, and GIF allowed.");

        // 3. Create the 'wwwroot/images' folder if it doesn't exist
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // 4. Generate a unique name so files don't overwrite each other
        // Example result: "550e8400-e29b-41d4-a716-446655440000.jpg"
        var uniqueFileName = Guid.NewGuid().ToString() + extension;
        var filePath = Path.Combine(folderPath, uniqueFileName);

        // 5. Save the file to the disk
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // 6. Return the URL path to the frontend
        // The frontend will grab this URL and send it back when creating the Announcement
        var urlPath = $"/images/{uniqueFileName}";
        
        return Ok(new { url = urlPath });
    } 
}