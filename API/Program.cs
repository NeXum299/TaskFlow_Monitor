using Microsoft.EntityFrameworkCore;
using TaskFlow_Monitor.Infrastructure.Contexts;
using Microsoft.OpenApi.Models;
using TaskFlow_Monitor.Domain.Interfaces.Repositories;
using TaskFlow_Monitor.Infrastructure.Repositories;
using TaskFlow_Monitor.Domain.Interfaces.Services;
using TaskFlow_Monitor.Domain.Services;
using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TaskFlow_Monitor.API.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Prometheus;
using TaskFlow_Monitor.API.Services;
using TaskFlow_Monitor.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://*:5000");

builder.Services.AddControllers();

builder.Services.AddHealthChecks()
    .AddSqlServer(
        connectionString: builder.Configuration.GetConnectionString(nameof(MyDbContext)) ?? "MyDbContext",
        name: "Database",
        tags: ["db", "sql"])
    .AddUrlGroup(new Uri(
        "http://localhost:9090"),
        "Prometheus",
        tags: ["monitor"])
    .AddProcessAllocatedMemoryHealthCheck(
        maximumMegabytesAllocated: 512,
        name: "Memory",
        tags: ["system"])
    .AddCheck<ApiHealthCheck>("api", tags: ["service"]);

builder.Services.AddHealthChecksUI(setup =>
{
    setup.AddHealthCheckEndpoint("API", "/health");
    setup.SetEvaluationTimeInSeconds(60);
    setup.SetApiMaxActiveRequests(3);
    setup.MaximumHistoryEntriesPerEndpoint(50);
}).AddInMemoryStorage();

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<ITaskHistoriesRepository, TaskHistoriesRepository>();
builder.Services.AddScoped<ITasksRepository, TasksRepository>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<ITasksService, TasksService>();
builder.Services.AddScoped<ITaskHistoriesService, TaskHistoriesService>();

builder.Services.AddSingleton<IMetricsService, MetricsService>();

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

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    Predicate = _ => true,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

app.MapHealthChecksUI(setup =>
{
    setup.UIPath = "/health-ui";
    setup.AddCustomStylesheet("wwwroot/healthchecks.css");
});

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

app.UseMiddleware<MetricsMiddleware>();
app.UseHttpMetrics();
app.MapMetrics("/metrics");
app.UseMetricServer(port: 81);

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
