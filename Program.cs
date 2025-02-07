using Microsoft.EntityFrameworkCore;
using mayanBoats;
using mayanBoats.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MayanDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services for controllers
builder.Services.AddControllers();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts(); // Use HSTS in non-development environments
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Map controllers
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Ensure controllers are mapped
    endpoints.MapRazorPages();  // Maps Razor Pages endpoints
});

app.Run();
