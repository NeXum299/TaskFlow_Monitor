using Microsoft.EntityFrameworkCore;
using TaskFlow_Monitor.Infrastructure.Contexts;
using Microsoft.OpenApi.Models;
using TaskFlow_Monitor.Domain.Interfaces.Repositories;
using TaskFlow_Monitor.Infrastructure.Repositories;
using TaskFlow_Monitor.Domain.Interfaces.Services;
using TaskFlow_Monitor.Domain.Services;
using System.Diagnostics.Metrics;
using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://*:5000");

builder.Services.AddControllers();

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<ITaskHistoriesRepository, TaskHistoriesRepository>();
builder.Services.AddScoped<ITasksRepository, TasksRepository>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<ITasksService, TasksService>();
builder.Services.AddScoped<ITaskHistoriesService, TaskHistoriesService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.00",
        Title = "TaskFlow Monitor",
        Description = "My TaskFlow Monitor",
        TermsOfService = new Uri("https://localhost/terms"),
        Contact = new OpenApiContact
        {
            Name = "Dmitry",
            Url = new Uri("https://localhost/contact")
        },
        License = new OpenApiLicense
        {
            Name = "License",
            Url = new Uri("https://localhost/license")
        }
    });
});

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(MyDbContext))));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskFlow Monitor v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseRouting();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<MyDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.Run();
