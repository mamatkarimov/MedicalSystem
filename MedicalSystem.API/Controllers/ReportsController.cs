using MedicalSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystem.API.Controllers
{
    [Authorize(Roles = "Admin,Accountant,ChiefDoctor")]
[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReportsController(AppDbContext context)
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
                Doctor = $"{g.Key.StaffProfile.LastName} {g.Key.StaffProfile.LastName}",
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