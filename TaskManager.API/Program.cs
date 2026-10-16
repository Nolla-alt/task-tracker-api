
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using TaskManager.Business.Services;
using TaskManager.Business.Services.Interfaces;
using TaskManager.Data.Context;
using TaskManager.Data.Repositories;
using TaskManager.Data.Repositories.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// ---- Register services ----

builder.Services.AddControllers();

/*builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));*/
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("TaskManager.Data")));

builder.Services.AddScoped<ICategoryRepository, CategoryRepositoryEfCore>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<IUserRepository, UserRepositoryEfCore>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ITaskItemRepository, TaskItemRepositoryEfCore>();
builder.Services.AddScoped<ITaskItemService, TaskItemService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// ---- Build the pipeline ----

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<TaskManager.API.Middleware.ExceptionHandlingMiddleware>();

//app.UseHttpsRedirection();//use https redirection middleware to redirect HTTP requests to HTTPS

app.MapControllers();

app.Run();


/*var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
app.UseMiddleware<TaskManager.API.Middleware.ExceptionHandlingMiddleware>();

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})

.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}


*/