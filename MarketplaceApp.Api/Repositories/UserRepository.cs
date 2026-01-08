using Dapper;
using Microsoft.Data.Sqlite;
using MarketplaceApp.Api.Models;

namespace MarketplaceApp.Api.Repositories;

public class UserRepository
{
    private readonly string _connectionString = "Data Source=marketplace.db";

    public IEnumerable<User> GetAllUsers()
    {
        using var connection = new SqliteConnection(_connectionString);
        return connection.Query<User>("SELECT * FROM User");
    }

    public User? GetUserById(int idUser)
    {
        using var connection = new SqliteConnection(_connectionString);

        return connection.QueryFirstOrDefault<User>(
            "SELECT * FROM User WHERE IdUser = @IdUser",
            new { IdUser = idUser }
        );
    }

    public void CreateUser(User user)
    {
        using var connection = new SqliteConnection(_connectionString);

        var sql = @"
            INSERT INTO User
                (Email, PasswordHash, FirstName, LastName, PhoneNumber)
            VALUES
                (@Email, @PasswordHash, @FirstName, @LastName, @PhoneNumber);
        ";

        connection.Execute(sql, user);
    }

    public void UpdateUser(int idUser, User user)
    {
        using var connection = new SqliteConnection(_connectionString);

        var sql = @"
            UPDATE User
            SET Email = @Email,
                FirstName = @FirstName,
                LastName = @LastName,
                PhoneNumber = @PhoneNumber
            WHERE IdUser = @IdUser;
        ";

        connection.Execute(sql, new
        {
            IdUser = idUser,
            user.Email,
            user.FirstName,
            user.LastName,
            user.PhoneNumber
        });
    }
}