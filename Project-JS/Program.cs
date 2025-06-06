using Microsoft.EntityFrameworkCore;
using Project_JS.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax; 
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // ✅ ТУТ — ПІСЛЯ routing, ДО authorization
app.UseAuthorization(); // Якщо не використовуєш .UseAuthentication() — норм

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Guest}/{id?}")
    .WithStaticAssets();



app.Run();
