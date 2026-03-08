// Program.cs
using Microsoft.EntityFrameworkCore;
using SmartPostOffice.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Add MVC (Controllers + Views)
builder.Services.AddControllersWithViews();

// 2. Add Database Context
builder.Services.AddDbContext<PostOfficeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddAuthentication("OfficerCookies")
    .AddCookie("OfficerCookies", options => {
        options.LoginPath = "/Auth/Login";        // redirect if not authenticated
        options.AccessDeniedPath = "/Auth/Login"; // redirect if unauthorised
        options.ExpireTimeSpan = TimeSpan.FromHours(8); // 8-hour session
    });
builder.Services.AddAuthorization();



var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();       
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();



