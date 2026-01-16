using MarketplaceApp.Api.Models;
using MarketplaceApp.Api.Models.DTOs;
using MarketplaceApp.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Generators;

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
                u.Id,
                u.Email,
                u.FirstName,
                u.LastName,
                u.PhoneNumber
            });

        return Ok(users);
    }

    // GET: api/User/5
    [HttpGet("{id:int}")]
    public IActionResult GetUserById(int id)
    {
        var user = _repo.GetById(id);
        if (user == null)
            return NotFound();

        return Ok(user);
    }


    // POST: api/User
    [HttpPost]
    public IActionResult CreateUser([FromBody] UserDTO dto)
    {
        // 1. Hash Password
        var hashed = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        // 2. Create Entity
        // (Ensure your User model uses 'Password' or 'PasswordHash' - stick to one!)
        var user = new User
        {
            Email = dto.Email,
            PasswordHash= hashed, // <--- Checked: Previous context used 'Password'
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber,
 
            // Create the profile immediately
            UserProfile = new UserProfile
            {
                Description = "New Member", 
                Pfp = "" // Empty string is better than null for frontend                
            }
        };

        try 
        {
            // 3. Save to DB (Wrapped in Try-Catch)
            _repo.Create(user);

            // 4. Map to Safe Response
            var response = new 
            {
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                ProfileDescription = user.UserProfile.Description
            };

            // 5. Return 201 Created
            // Ensure "GetUserById" exists in this controller!
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, response);
        }
        catch (InvalidOperationException ex)
        {
            // 6. Handle Duplicates politely
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            // 7. Handle real crashes
            return StatusCode(500, "An error occurred while registering.");
        }
    }
    // PUT: api/User/5
    [HttpPut("{id:int}")]
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
    
    [HttpGet("{email}")]
    public IActionResult GetUserByEmail(string email)
    {
        var user = _repo.GetByEmail(email);
        if (user == null)
            return NotFound();
        return Ok(user);
    }

    [HttpPost("login")]
    public IActionResult Authenticate([FromBody] UserLoginDTO dto)
    {
        bool value =_repo.Login(dto.email, dto.password);
        if (value)
        {
            return Ok();
        }
        return BadRequest();
    }
}
