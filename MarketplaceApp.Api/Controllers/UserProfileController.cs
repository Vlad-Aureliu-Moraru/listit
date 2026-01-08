using MarketplaceApp.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserProfileController : ControllerBase
{
    private readonly UserProfileRepository _repo;

    public UserProfileController(UserProfileRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public IActionResult GetAllUserProfiles()
    {
        var profiles = _repo.GetAllUserProfiles();
        return Ok(profiles);
    }
}