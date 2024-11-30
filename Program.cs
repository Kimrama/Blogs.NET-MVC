using Microsoft.EntityFrameworkCore;
using blog.Data;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services DbContext
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Handle JSON
builder.Services.AddControllers().AddJsonOptions(options => {options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Blogs}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
