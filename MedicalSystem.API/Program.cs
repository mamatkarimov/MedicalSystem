using MedicalSystem.API.Endpoints;
using MedicalSystem.Infrastructure;
using MedicalSystem.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddControllers();

// Authentication setup
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:5001"; // Change as needed
        options.RequireHttpsMetadata = false;
        options.Audience = "medical_api";
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); // for attribute routing if needed
app.MapAuthEndpoints();
// Temporary minimal endpoint to test
app.MapGet("/", () => "MedicalSystem API is running").AllowAnonymous();

app.Run();
