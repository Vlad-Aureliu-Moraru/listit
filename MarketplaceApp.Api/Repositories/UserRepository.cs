using Dapper;
using Microsoft.Data.Sqlite;
using MarketplaceApp.Api.Models;

namespace MarketplaceApp.Api.Repositories;

public class UserRepository
{
    private readonly string _connectionString;

    public UserRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection")!;
    }

    public IEnumerable<User> GetAllUsers()
    {
        using var db = new SqliteConnection(_connectionString);
        return db.Query<User>("SELECT * FROM Users");
    }

    public User GetUserById(int id)
    {
        using var db = new SqliteConnection(_connectionString);
        return db.Query<User>("SELECT * FROM Users WHERE Id = @id", new
        { id
        }).FirstOrDefault();
    }

    public int CreateUser(User user)
    {
        using var db = new SqliteConnection(_connectionString);
        // SQLite uses last_insert_rowid() to get the new ID
        string sql = "INSERT INTO Users (Email, PasswordHash) VALUES (@Email, @PasswordHash); SELECT last_insert_rowid();";
        return db.ExecuteScalar<int>(sql, user);
    }

    public void UpdateUser(User user, int id)
    {
        using var db = new SqliteConnection(_connectionString);
        string sql = "UPDATE Users SET Email = @Email, PasswordHash = @PasswordHash WHERE IdUser = @id";
    
        db.Execute(sql, new { user.Email, user.PasswordHash, id });
    }
    
}