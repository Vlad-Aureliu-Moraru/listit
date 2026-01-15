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

        return Ok(new
        {
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.PhoneNumber
        });
    }


    // POST: api/User
    [HttpPost]
    public IActionResult CreateUser([FromBody] UserDTO dto)
    {
        // 1. Hash Password
        var hashed = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        // 2. Create Entity
        var user = new User
        {
            Email = dto.Email,
            PasswordHash = hashed,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber,
     
            // Create the profile immediately
            UserProfile = new UserProfile
            {
                Description = "New Member", 
                Pfp = ""                    
            }
        };

        // 3. Save to DB
        _repo.Create(user);

        // 4. FIX: Map to a Safe Response Object (DTO)
        // This creates a clean object without circular links or passwords
        var response = new 
        {
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.PhoneNumber,
            // Flatten the profile data so Angular can read it easily
            ProfileDescription = user.UserProfile.Description,
            ProfilePictureUrl = user.UserProfile.Pfp
        };

        // 5. Return the clean response
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, response);
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
