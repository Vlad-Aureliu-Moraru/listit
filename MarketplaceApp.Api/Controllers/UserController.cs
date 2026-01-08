using MarketplaceApp.Api.Models;
using MarketplaceApp.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserRepository _repo;
    public UserController(UserRepository repo) => _repo = repo;

    [HttpGet]
    public IActionResult GetAllUsers()
    {
        return Ok(_repo.GetAllUsers());
    }

    [HttpGet("{id}")]
    public IActionResult GetUser([FromRoute] int id)
    {
        return Ok(_repo.GetUserById(id));
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] User user)
    {
        _repo.CreateUser(user);
        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult UpdateUser([FromRoute]  int id, [FromBody] User user)
    {
        _repo.UpdateUser(id,user);
        return Ok();
    }
    


}