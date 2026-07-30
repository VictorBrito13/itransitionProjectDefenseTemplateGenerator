using System.Text.Json;
using System.Text.Json.Serialization;
using ItransitionTemplates.Data;
using ItransitionTemplates.Middleware;
using ItransitionTemplates.Services.Admin;
using ItransitionTemplates.Services.Question;
using ItransitionTemplates.Services.QuestionOption;
using ItransitionTemplates.Services.Response;
using ItransitionTemplates.Services.Template;
using ItransitionTemplates.Services.Topic;
using ItransitionTemplates.Services.User;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
});

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.Cookie.Name = ".AspNetCore.Antiforgery";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});


string? dbConnection = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(dbConnection))
{
    throw new InvalidOperationException(
        "ConnectionStrings:DefaultConnection is missing. " +
        "Set Cloud Run env var ConnectionStrings__DefaultConnection " +
        "(not _DB_CONNECTION_STRING).");
}

MySqlServerVersion serverVersion = new MySqlServerVersion(new Version(8,0,46));
builder.Services.AddDbContext<ApplicationDBContext>(options => {
    options.UseMySql(dbConnection, serverVersion);
    if (builder.Environment.IsDevelopment()) {
        options.EnableSensitiveDataLogging();
    }
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddScoped<IUserService, ItransitionTemplates.Services.User.User>();
builder.Services.AddScoped<ITopic, ItransitionTemplates.Services.Topic.Topic>();
builder.Services.AddScoped<ITemplate, ItransitionTemplates.Services.Template.Template>();
builder.Services.AddScoped<IQuestion, ItransitionTemplates.Services.Question.Question>();
builder.Services.AddScoped<IAdmin, ItransitionTemplates.Services.Admin.Admin>();
builder.Services.AddScoped<IQuestionOption, ItransitionTemplates.Services.QuestionOption.QuestionOption>();
builder.Services.AddScoped<IResponse, ItransitionTemplates.Services.Response.Response>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public partial class Program { }
