using MedicalSystem.Application.Models.Requests;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystem.API.Endpoints
{
    public static class PatientEndpoints
    {
        public static void MapPatientEndpoints(this WebApplication app)
        {
            app.MapPost("/api/patients", async ([FromBody] RegisterPatientRequest request, AppDbContext db) =>
            {
                var patient = new Patient
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    DateOfBirth = request.DateOfBirth,
                    Gender = request.Gender                    
                };

                db.Patients.Add(patient);
                await db.SaveChangesAsync();

                return Results.Ok(patient.Id);
            });

            app.MapGet("/api/patients", async (AppDbContext db) =>
            {
                var patients = await db.Patients
                    .Select(p => new
                    {
                        p.Id,
                        p.FirstName,
                        p.LastName,
                        p.DateOfBirth,
                        p.Gender
                    })
                    .ToListAsync();

                return Results.Ok(patients);
            });

        }
    }
}
