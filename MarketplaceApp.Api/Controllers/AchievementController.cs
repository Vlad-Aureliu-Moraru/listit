using Microsoft.AspNetCore.Mvc;
using MarketplaceApp.Api.Models;
using MarketplaceApp.Api.Models.AnnouncementDTOs;
using MarketplaceApp.Api.Repositories;

namespace MarketplaceApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AchievementController : ControllerBase
{
    private readonly AchievementRepository _repo;

    public AchievementController(AchievementRepository repo)
    {
        _repo = repo;
    }

    // GET: api/Achievement
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_repo.GetAll());
    }

    // POST: api/Achievement (Create a new badge type, e.g. "Top Seller")
    [HttpPost]
    public IActionResult Create([FromBody] Achievement achievement)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        _repo.Create(achievement);
        return CreatedAtAction(nameof(GetAll), new { id = achievement.AchievementId }, achievement);
    }

    
    // Assigns an existing badge to a user
    [HttpPost("assign")]
    public IActionResult Assign([FromBody] AssignAchievementDTO assignAchievementDTO)
    {
        try
        {
            _repo.AssignAchievementToUser(assignAchievementDTO.UserId,assignAchievementDTO.AchievementId);
            return Ok(new { message = "Badge assigned successfully!" });
        }
        catch (Exception ex)
        {
            return BadRequest("Could not assign badge. Check if User/Badge IDs exist.");
        }
    }

    // GET: api/Achievement/user/1
    // Get all badges for User #1
    [HttpGet("user/{userId}")]
    public IActionResult GetForUser(int userId)
    {
        var badges = _repo.GetAchievementsByUser(userId);
        return Ok(badges);
    }
}