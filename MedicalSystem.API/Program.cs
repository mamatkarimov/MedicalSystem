using MedicalSystem.API.Endpoints;
using MedicalSystem.Infrastructure;
using MedicalSystem.Application;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using System.Text.Json;
using MedicalSystem.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddControllers();

var jwtKey = builder.Configuration["Jwt:Key"];
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

// Authentication setup
//builder.Services.AddAuthentication("Bearer")
//    .AddJwtBearer("Bearer", options =>
//    {
//        options.Authority = "https://localhost:5001"; // Change as needed
//        options.RequireHttpsMetadata = false;
//        options.Audience = "medical_api";
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,

//            ValidIssuer = builder.Configuration["Jwt:Issuer"],
//            ValidAudience = builder.Configuration["Jwt:Audience"],
//            IssuerSigningKey = key // ✅ MUST MATCH the key used in AuthEndpoints
//        };
//    });

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("Patient", policy =>
//    {
//        policy.RequireRole("Patient");
//    });

//    // You may add other roles too for later use
//    options.AddPolicy("Doctor", policy =>
//    {
//        policy.RequireRole("Doctor");
//    });
//});

//builder.Services.AddAuthentication("Bearer")
//    .AddJwtBearer("Bearer", options =>
//    {
//        options.Authority = "http://localhost:8080/realms/medical-realm";
//        options.RequireHttpsMetadata = false;

//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateAudience = false,
//            NameClaimType = "preferred_username",
//            RoleClaimType = ClaimTypes.Role
//        };

//        options.Events = new JwtBearerEvents
//        {
//            OnTokenValidated = context =>
//            {
//                var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
//                var roleClaims = context.Principal.FindAll("realm_access.roles");

//                foreach (var role in roleClaims)
//                {
//                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.Value));
//                }

//                return Task.CompletedTask;
//            }
//        };
//    });

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "http://localhost:8080/realms/medical-realm";
        options.RequireHttpsMetadata = false; // Only for development!

        // Better token validation parameters
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false, // Should typically be true
            //ValidateIssuer = true,
            //ValidIssuer = "http://localhost:8080/realms/medical-realm",
            
            //ValidAudience = "account", // Or your client ID
            //ValidateLifetime = true,
            NameClaimType = "preferred_username",
            RoleClaimType = ClaimTypes.Role
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                if (context.Principal?.Identity is ClaimsIdentity claimsIdentity)
                {
                    // Better way to extract realm roles from Keycloak token
                    var realmAccess = context.Principal.FindFirst("realm_access")?.Value;
                    if (!string.IsNullOrEmpty(realmAccess))
                    {
                        var realmAccessObj = JsonSerializer.Deserialize<RealmAccess>(realmAccess);
                        foreach (var role in realmAccessObj?.Roles ?? Enumerable.Empty<string>())
                        {
                            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
                        }
                    }
                }
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            }
        };
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
app.MapUserEndpoints();
app.MapAppointmentEndpoints();

app.MapGet("/", () => "MedicalSystem API is running").AllowAnonymous();

app.Run();

