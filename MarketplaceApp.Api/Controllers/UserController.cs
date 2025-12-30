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
    public IActionResult Get() => Ok(_repo.GetAllUsers());

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        User found = _repo.GetUserById(id);
        if (found == null)
            return NotFound();
        return Ok(found+""+found);

    }
    [HttpPost]
    public IActionResult Create(User user)
    {
        int newId = _repo.CreateUser(user);
        return Ok(new { id = newId });
    }}