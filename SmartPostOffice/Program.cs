// Program.cs
using Microsoft.EntityFrameworkCore;
using SmartPostOffice.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Add MVC (Controllers + Views)
builder.Services.AddControllersWithViews();

// 2. Add Database Context
builder.Services.AddDbContext<PostOfficeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();       
app.UseRouting();
app.UseAuthorization();


