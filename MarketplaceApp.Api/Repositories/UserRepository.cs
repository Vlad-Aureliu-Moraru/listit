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
        return _context.Users.AsNoTracking().ToList();
    }

    public User? GetById(int idUser)
    {
        return _context.Users.Find(idUser);
    }

    public User? GetByEmail(string email)
    {
        return _context.Users.FirstOrDefault(u => u.Email == email);
    }

    public void Create(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public void Delete(int idUser)
    {
        var user = _context.Users.Find(idUser);
        if (user == null) return;

        _context.Users.Remove(user);
        _context.SaveChanges();
    }
}