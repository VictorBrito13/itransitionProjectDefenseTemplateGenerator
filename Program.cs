using System.Text.Json;
using System.Text.Json.Serialization;
using ItransitionTemplates.Data;
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
builder.Services.AddControllersWithViews();


string dbConnectionTest = "Server=localhost;Database=itransition_template_manager;User=root;Password=root";
string dbConnection = "Server=template-manager.mysql.database.azure.com;Database=itransition_template_manager;User=user;Password=$Us3r$13";
MySqlServerVersion serverVersion = new MySqlServerVersion(new Version(8,0,38));
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseMySql(dbConnectionTest, serverVersion).EnableSensitiveDataLogging());

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

builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
