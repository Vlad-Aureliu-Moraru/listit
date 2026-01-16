using MarketplaceApp.Api.DB;
using MarketplaceApp.Api.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Add services
// (Removed duplicate AddControllers call here)
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

// 3. Database Context
builder.Services.AddDbContext<MarketplaceDbContext>(options =>
    options.UseSqlite(
        "Data Source=marketplace.db", 
        b => b.MigrationsAssembly("MarketplaceApp.Api")
    ));

// 4. Repositories (Direct Registration)
// Since you have no interfaces, we register the class to itself.
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserProfileRepository>();
builder.Services.AddScoped<CategoryRepository>();

// MAKE SURE these names match your actual class filenames exactly!
// (e.g. is it "AnnouncementsRepository" or "AnnouncementRepository"?)
builder.Services.AddScoped<AnnouncementsRepository>(); 
builder.Services.AddScoped<AchievementRepository>();

// 5. Controller Configuration
// Merged the JSON configuration here to handle loops (ReferenceHandler.IgnoreCycles)
builder.Services.AddControllers()
    .AddJsonOptions(x => 
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

var app = builder.Build();

// 6. Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

// IMPORTANT: This enables the image URLs to work!
app.UseStaticFiles(); 

app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();