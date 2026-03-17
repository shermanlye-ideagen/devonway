using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

app.MapGet("/health", () => new { Status = "healthy", Product = "DevonWay", Version = "1.0.0" });

app.MapGet("/incidents", () => new[]
{
    new { Id = 1, Title = "Slip hazard in warehouse B", Severity = "High", Status = "Open" },
    new { Id = 2, Title = "PPE compliance check overdue", Severity = "Medium", Status = "In Progress" },
    new { Id = 3, Title = "Fire extinguisher inspection", Severity = "Low", Status = "Resolved" }
});

app.MapPost("/incidents", (IncidentRequest req) =>
{
    // INTENTIONAL ISSUE: No input validation for demo
    return new { Message = $"Incident '{req.Title}' created", Severity = req.Severity };
});

app.Run();

record IncidentRequest(string Title, string Severity, string Description);
