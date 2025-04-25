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
    [Authorize]
[Route("api/[controller]")]
[ApiController]
public class PaymentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PaymentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Admin,Accountant")]
    [HttpGet("services")]
    public async Task<ActionResult<IEnumerable<Service>>> GetServices()
    {
        return await _context.Services.Where(s => s.IsActive).ToListAsync();
    }

    [Authorize(Roles = "Admin,Accountant")]
    [HttpPost("services")]
    public async Task<ActionResult<Service>> CreateService(Service service)
    {
        _context.Services.Add(service);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetService", new { id = service.ServiceID }, service);
    }

    [Authorize(Roles = "Admin,Accountant")]
    [HttpPut("services/{id}")]
    public async Task<IActionResult> UpdateService(int id, Service service)
    {
        if (id != service.ServiceID)
        {
            return BadRequest();
        }

        _context.Entry(service).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ServiceExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [Authorize(Roles = "Admin,Accountant,Cashier")]
    [HttpPost("invoices")]
    public async Task<ActionResult<Invoice>> CreateInvoice(CreateInvoiceRequest request)
    {
        var patient = await _context.Patients.FindAsync(request.PatientID);
        if (patient == null || !patient.IsActive)
        {
            return NotFound("Patient not found");
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var invoice = new Invoice
        {
            PatientID = request.PatientID,
            CreatedByID = userId,
            DueDate = DateTime.UtcNow.AddDays(30)
        };

        decimal totalAmount = 0;

        foreach (var item in request.Items)
        {
            var service = await _context.Services.FindAsync(item.ServiceID);
            if (service == null || !service.IsActive)
            {
                continue; // or return error
            }

            var detail = new InvoiceDetail
            {
                ServiceID = item.ServiceID,
                Quantity = item.Quantity,
                UnitPrice = service.Price,
                Discount = item.Discount
            };

            totalAmount += (service.Price * item.Quantity) - item.Discount;

            invoice.InvoiceDetails.Add(detail);
        }

        invoice.TotalAmount = totalAmount;

        // If payment method provided, process payment immediately
        if (!string.IsNullOrEmpty(request.PaymentMethod))
        {
            var payment = new Payment
            {
                Amount = totalAmount,
                PaymentMethod = request.PaymentMethod,
                ReceivedByID = userId
            };

            invoice.Payments.Add(payment);
            invoice.PaidAmount = totalAmount;
            invoice.Status = "Paid";
        }

        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetInvoice", new { id = invoice.InvoiceID }, invoice);
    }

    [Authorize(Roles = "Admin,Accountant,Cashier")]
    [HttpGet("invoices/{id}")]
    public async Task<ActionResult<Invoice>> GetInvoice(int id)
    {
        var invoice = await _context.Invoices
            .Include(i => i.Patient)
            .Include(i => i.CreatedBy)
            .Include(i => i.InvoiceDetails)
                .ThenInclude(d => d.Service)
            .Include(i => i.Payments)
            .FirstOrDefaultAsync(i => i.InvoiceID == id);

        if (invoice == null)
        {
            return NotFound();
        }

        return invoice;
    }

    [Authorize(Roles = "Admin,Cashier")]
    [HttpPost("payments")]
    public async Task<ActionResult<Payment>> ProcessPayment(ProcessPaymentRequest request)
    {
        var invoice = await _context.Invoices.FindAsync(request.InvoiceID);
        if (invoice == null)
        {
            return NotFound("Invoice not found");
        }

        if (invoice.Status == "Cancelled")
        {
            return BadRequest("Invoice is cancelled");
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var payment = new Payment
        {
            InvoiceID = request.InvoiceID,
            Amount = request.Amount,
            PaymentMethod = request.PaymentMethod,
            ReceivedByID = userId,
            Notes = request.Notes
        };

        invoice.PaidAmount += request.Amount;

        if (invoice.PaidAmount >= invoice.TotalAmount)
        {
            invoice.Status = "Paid";
        }
        else if (invoice.PaidAmount > 0)
        {
            invoice.Status = "PartiallyPaid";
        }

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetPayment", new { id = payment.PaymentID }, payment);
    }

    [Authorize(Roles = "Admin,Accountant")]
    [HttpGet("reports/payments")]
    public async Task<ActionResult<object>> GetPaymentsReport(DateTime fromDate, DateTime toDate)
    {
        var payments = await _context.Payments
            .Include(p => p.Invoice)
                .ThenInclude(i => i.Patient)
            .Include(p => p.ReceivedBy)
            .Where(p => p.PaymentDate >= fromDate && p.PaymentDate <= toDate)
            .ToListAsync();

        var totalAmount = payments.Sum(p => p.Amount);

        return new
        {
            TotalAmount = totalAmount,
            Payments = payments
        };
    }

    private bool ServiceExists(int id)
    {
        return _context.Services.Any(e => e.ServiceID == id);
    }
}
}