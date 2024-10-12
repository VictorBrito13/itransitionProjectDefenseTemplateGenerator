using ItransitionTemplates.Data;
using ItransitionTemplates.Services.User;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string dbConnection = "Server=localhost;Database=itransition_template_manager;User=root;Password=root";
MySqlServerVersion serverVersion = new MySqlServerVersion(new Version(8,0,38));
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseMySql(dbConnection, serverVersion));

builder.Services.AddScoped<IUserService, ItransitionTemplates.Services.User.User>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
