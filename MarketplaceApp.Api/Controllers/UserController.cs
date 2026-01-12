using MarketplaceApp.Api.Models;
using MarketplaceApp.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserRepository _repo;

    public UserController(UserRepository repo)
    {
        _repo = repo;
    }

    // GET: api/User
    [HttpGet]
    public IActionResult GetAllUsers()
    {
        var users = _repo.GetAll()
            .Select(u => new
            {
                u.IdUser,
                u.Email,
                u.FirstName,
                u.LastName,
                u.PhoneNumber
            });

        return Ok(users);
    }

    // GET: api/User/5
    [HttpGet("{id}")]
    public IActionResult GetUserById(int id)
    {
        var user = _repo.GetById(id);
        if (user == null)
            return NotFound();

        return Ok(new
        {
            user.IdUser,
            user.Email,
            user.FirstName,
            user.LastName,
            user.PhoneNumber
        });
    }

    // POST: api/User
    [HttpPost]
    public IActionResult CreateUser([FromBody] User user)
    {
        if (string.IsNullOrWhiteSpace(user.PasswordHash))
            return BadRequest("Password is required");

        _repo.Create(user);

        return CreatedAtAction(
            nameof(GetUserById),
            new { id = user.IdUser },
            new
            {
                user.IdUser,
                user.Email,
                user.FirstName,
                user.LastName,
                user.PhoneNumber
            });
    }

    // PUT: api/User/5
    [HttpPut("{id}")]
    public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
    {
        var user = _repo.GetById(id);
        if (user == null)
            return NotFound();

        user.Email = updatedUser.Email;
        user.FirstName = updatedUser.FirstName;
        user.LastName = updatedUser.LastName;
        user.PhoneNumber = updatedUser.PhoneNumber;

        _repo.Update(user);

        return NoContent();
    }

    // DELETE: api/User/5
    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        _repo.Delete(id);
        return NoContent();
    }
}
