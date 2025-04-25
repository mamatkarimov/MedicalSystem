using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MedicalSystemAPI.Models;
using MedicalSystemAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using MedicalSystemAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystemAPI.Controllers
{
    [Authorize(Roles = "Admin,Accountant,ChiefDoctor")]
[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ReportsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("financial")]
    public async Task<ActionResult<object>> GetFinancialReport(DateTime fromDate, DateTime toDate)
    {
        var invoices = await _context.Invoices
            .Include(i => i.Patient)
            .Include(i => i.InvoiceDetails)
                .ThenInclude(d => d.Service)
            .Where(i => i.InvoiceDate >= fromDate && i.InvoiceDate <= toDate)
            .ToListAsync();

        var totalInvoiced = invoices.Sum(i => i.TotalAmount);
        var totalPaid = invoices.Sum(i => i.PaidAmount);

        return new
        {
            TotalInvoiced = totalInvoiced,
            TotalPaid = totalPaid,
            Outstanding = totalInvoiced - totalPaid,
            Invoices = invoices
        };
    }

    [HttpGet("appointments")]
    public async Task<ActionResult<object>> GetAppointmentsReport(DateTime fromDate, DateTime toDate)
    {
        var appointments = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Where(a => a.AppointmentDate >= fromDate && a.AppointmentDate <= toDate)
            .ToListAsync();

        var groupedByStatus = appointments
            .GroupBy(a => a.Status)
            .Select(g => new
            {
                Status = g.Key,
                Count = g.Count()
            });

        var groupedByDoctor = appointments
            .GroupBy(a => a.Doctor)
            .Select(g => new
            {
                Doctor = $"{g.Key.LastName} {g.Key.FirstName}",
                Count = g.Count()
            });

        return new
        {
            TotalAppointments = appointments.Count,
            ByStatus = groupedByStatus,
            ByDoctor = groupedByDoctor,
            Appointments = appointments
        };
    }

    [HttpGet("hospitalizations")]
    public async Task<ActionResult<object>> GetHospitalizationsReport(DateTime fromDate, DateTime toDate)
    {
        var hospitalizations = await _context.Hospitalizations
            .Include(h => h.Patient)
            .Include(h => h.AttendingDoctor)
            .Include(h => h.Bed)
                .ThenInclude(b => b.Ward)
            .Where(h => h.AdmissionDate >= fromDate && h.AdmissionDate <= toDate)
            .ToListAsync();

        var averageStay = hospitalizations
            .Where(h => h.DischargeDate.HasValue)
            .Average(h => (h.DischargeDate.Value - h.AdmissionDate).TotalDays);

        var groupedByWard = hospitalizations
            .GroupBy(h => h.Bed.Ward)
            .Select(g => new
            {
                Ward = g.Key.WardNumber,
                Count = g.Count()
            });

        return new
        {
            TotalHospitalizations = hospitalizations.Count,
            AverageStayDays = averageStay,
            ByWard = groupedByWard,
            Hospitalizations = hospitalizations
        };
    }
}
}