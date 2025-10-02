using Microsoft.EntityFrameworkCore;
using TaskFlow_Monitor.Infrastructure.Contexts;
using Microsoft.OpenApi.Models;
using TaskFlow_Monitor.Domain.Interfaces.Repositories;
using TaskFlow_Monitor.Infrastructure.Repositories;
using TaskFlow_Monitor.Domain.Interfaces.Services;
using TaskFlow_Monitor.Domain.Services;
using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Prometheus;
using TaskFlow_Monitor.API.Metrics;
using TaskFlow_Monitor.API.Interfaces.Metrics;
using TaskFlow_Monitor.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://*:80");

builder.Services.AddControllers();

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<ITaskHistoriesRepository, TaskHistoriesRepository>();
builder.Services.AddScoped<ITasksRepository, TasksRepository>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<ITasksService, TasksService>();
builder.Services.AddScoped<ITaskHistoriesService, TaskHistoriesService>();

builder.Services.AddSingleton<CustomMetrics>();
builder.Services.AddSingleton<ICustomMetrics, CustomMetrics>();

builder.Services.AddScoped<ITaskMetricsService, TaskMetricsService>();
builder.Services.AddHostedService<MetricsBackgroundService>();

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

builder.Services.AddHttpClient();

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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<MyDbContext>();
        context.Database.Migrate();
        Console.WriteLine("Database migrations applied successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while applying migrations: {ex.Message}");
    }
}

app.UseHttpMetrics();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapMetrics();
});

app.Run();
