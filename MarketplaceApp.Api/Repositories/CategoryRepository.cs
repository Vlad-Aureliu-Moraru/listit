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
        return _context.Categories.ToList();
    }

    public Category? GetById(int id)
    {
        return _context.Categories.Find(id);
    }

    public void Create(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();
    }

    public void Update(Category category)
    {
        _context.Categories.Update(category);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var category = _context.Categories.Find(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }
}