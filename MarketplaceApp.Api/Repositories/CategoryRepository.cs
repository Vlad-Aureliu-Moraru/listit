using MarketplaceApp.Api.DB; // or .Data, depending on your namespace
using MarketplaceApp.Api.Models;

namespace MarketplaceApp.Api.Repositories;

public class CategoryRepository 
{
    private readonly MarketplaceDbContext _context;

    public CategoryRepository(MarketplaceDbContext context)
    {
        _context = context;
    }

    public List<Category> GetAll()
    {
        return _context.Category.ToList();
    }

    public Category? GetById(int id)
    {
        return _context.Category.Find(id);
    }

    public void Create(Category category)
    {
        _context.Category.Add(category);
        _context.SaveChanges();
    }

    public void Update(Category category)
    {
        _context.Category.Update(category);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var category = _context.Category.Find(id);
        if (category != null)
        {
            _context.Category.Remove(category);
            _context.SaveChanges();
        }
    }
}