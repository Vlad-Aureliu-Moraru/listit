using MarketplaceApp.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserProfileController : ControllerBase
{
    public UserProfileController(UserProfileRepository profileRepo)
    {
        _profileRepo = profileRepo; // <--- You likely missed this part!
    } 
    private readonly UserProfileRepository _profileRepo;
    [HttpPut("update-bio/{userId}")]
    public IActionResult UpdateBio(int userId, [FromBody] string newDescription)
    {
        var profile = _profileRepo.GetByUserId(userId);
    
        if (profile == null) return NotFound("Profile not found");

        profile.Description = newDescription;
        _profileRepo.Update(profile);

        return Ok(new { message = "Bio updated", description = profile.Description });
    } 
}