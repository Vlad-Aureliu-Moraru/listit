using MarketplaceApp.Api.DB;
using MarketplaceApp.Api.Models;
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

    public User? GetById(int idUser)
    {
        return _context.User.Find(idUser);
    }

    public User? GetByEmail(string email)
    {
        return _context.User.FirstOrDefault(u => u.Email == email);
    }

    public void Create(User user)
    {
        User existingEmail= GetByEmail(user.Email);
        if (existingEmail == null)
        {
            _context.User.Add(user);
            _context.SaveChanges();
            return;    
        }
        throw new Exception($"User with email {user.Email} was found");

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