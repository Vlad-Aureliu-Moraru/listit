using Dapper;
using MarketplaceApp.Api.Models;
using Microsoft.Data.Sqlite;

namespace MarketplaceApp.Api.Repositories;

public class UserProfileRepository
{
    private readonly string _connectionString = "Data Source=marketplace.db";

    public IEnumerable<UserProfile> GetAllUserProfiles()
    {
        using var connection = new SqliteConnection(_connectionString);
        return connection.Query<UserProfile>("SELECT * FROM UserProfiles");
    }

    public UserProfile? GetUserProfileByUserId(int userId)
    {
        using var connection = new SqliteConnection(_connectionString);

        return connection.QueryFirstOrDefault<UserProfile>(
            "SELECT * FROM UserProfiles WHERE UserId = @UserId",
            new { UserId = userId }
        );
    }

    public void CreateUserProfile(UserProfile profile)
    {
        using var connection = new SqliteConnection(_connectionString);

        var sql = @"
            INSERT INTO UserProfiles (UserId, Description, ProfilePictureUrl)
            VALUES (@UserId, @Description, @ProfilePictureUrl);
        ";

        connection.Execute(sql, profile);
    }

    public void UpdateUserProfile(int userId, UserProfile profile)
    {
        using var connection = new SqliteConnection(_connectionString);

        var sql = @"
            UPDATE UserProfiles
            SET Description = @Description,
                ProfilePictureUrl = @ProfilePictureUrl
            WHERE UserId = @UserId;
        ";

        connection.Execute(sql, new
        {
            UserId = userId,
            profile.Description,
            profile.ProfilePictureUrl
        });
    }
}