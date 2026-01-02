namespace MarketplaceApp.Api.Models;

public class User
{
    public int IdUser { get; set; } 
    public string Email { get; set; }
    public string PasswordHash{ get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber{ get; set; }
    
}