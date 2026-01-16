using MarketplaceApp.Api.DB;
using MarketplaceApp.Api.Models;
using MarketplaceApp.Api.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceApp.Api.Repositories;

public class UserRepository
{
    private readonly MarketplaceDbContext _context;

    public UserRepository(MarketplaceDbContext context)
    {
        _context = context;
    }

    public IEnumerable<User> GetAll()
    {
        return _context.User.AsNoTracking().ToList();
    }

    public User? GetById(int id)
    {
        return _context.User
            .Include(u => u.UserProfile) 
            .FirstOrDefault(u => u.Id == id);
    }
    
    public User? GetByEmail(string email)
    {
        return _context.User.Include(u=>u.UserProfile ).FirstOrDefault(u => u.Email == email);
    }

    public bool Login(string email, string password)
    {
        User found =  _context.User.FirstOrDefault(u => u.Email == email);
        if (found == null)
        {
            return false;
        }
        bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, found.PasswordHash);
        return isValidPassword;
    }
    

    public void Create(User user)
    {
        // 1. Check if user exists (Guard Clause)
        var existingUser = GetByEmail(user.Email);

        if (existingUser != null)
        {
            // Use a specific exception type so the Controller can catch it
            throw new InvalidOperationException($"User with email {user.Email} already exists.");
        }

        // 2. Save the new user
        _context.User.Add(user);
        _context.SaveChanges();
    }

    public void Update(User user)
    {
        _context.User.Update(user);
        _context.SaveChanges();
    }

    public void Delete(int idUser)
    {
        var user = _context.User.Find(idUser);
        if (user == null) return;

        _context.User.Remove(user);
        _context.SaveChanges();
    }
}