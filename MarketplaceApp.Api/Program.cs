using MarketplaceApp.Api.DB;
using MarketplaceApp.Api.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Enable CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// 3. Entity Framework Core (NO SQL, NO Dapper)
builder.Services.AddDbContext<MarketplaceDbContext>(options =>
    options.UseSqlite(
        "Data Source=marketplace.db", 
        b => b.MigrationsAssembly("MarketplaceApp.Api") // Explicitly tell EF where to look
    ));
// 4. Repositories
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserProfileRepository>();
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped< AnnouncementsRepository>();

builder.Services.AddControllers()
    .AddJsonOptions(x => 
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

var app = builder.Build();

// 5. Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();