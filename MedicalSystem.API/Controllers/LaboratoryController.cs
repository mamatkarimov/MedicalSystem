using MedicalSystem.API.Models.Requests;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MedicalSystem.API.Controllers
{
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class LaboratoryController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public LaboratoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Admin,Doctor")]
    [HttpGet("test-types")]
    public async Task<ActionResult<IEnumerable<LabTestType>>> GetTestTypes()
    {
        return await _context.LabTestTypes.ToListAsync();
    }

    [Authorize(Roles = "Admin,Doctor")]
    [HttpPost("orders")]
    public async Task<ActionResult<LabOrder>> CreateLabOrder(CreateLabOrderRequest request)
    {
        var patient = await _context.Patients.FindAsync(request.PatientID);
        if (patient == null || !patient.IsActive)
        {
            return NotFound("Patient not found");
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var order = new LabOrder
        {
            PatientID = request.PatientID,
            OrderedByID = userId,
            Priority = request.Priority,
            Notes = request.Notes
        };

        _context.LabOrders.Add(order);
        await _context.SaveChangesAsync();

        foreach (var testTypeId in request.TestTypeIDs)
        {
            var testType = await _context.LabTestTypes.FindAsync(testTypeId);
            if (testType == null)
            {
                continue; // or return error
            }

            var orderDetail = new LabOrderDetail
            {
                OrderID = order.OrderID,
                TestTypeID = testTypeId,
                ReferenceRange = testType.NormalRange
            };

            _context.LabOrderDetails.Add(orderDetail);
        }

        await _context.SaveChangesAsync();

        return CreatedAtAction("GetLabOrder", new { id = order.OrderID }, order);
    }

    [Authorize(Roles = "Admin,Doctor,LabTechnician")]
    [HttpGet("orders")]
    public async Task<ActionResult<IEnumerable<LabOrder>>> GetLabOrders()
    {
        var query = _context.LabOrders
            .Include(o => o.Patient)
            .Include(o => o.OrderedBy)
            .Include(o => o.LabOrderDetails)
                .ThenInclude(d => d.TestType)
            .AsQueryable();

        // Lab technicians see only pending/in progress orders
        if (User.IsInRole("LabTechnician"))
        {
            query = query.Where(o => o.Status == "Pending" || o.Status == "InProgress");
        }

        return await query.ToListAsync();
    }

    [Authorize(Roles = "Admin,Doctor,LabTechnician")]
    [HttpGet("orders/{id}")]
    public async Task<ActionResult<LabOrder>> GetLabOrder(int id)
    {
        var order = await _context.LabOrders
            .Include(o => o.Patient)
            .Include(o => o.OrderedBy)
            .Include(o => o.LabOrderDetails)
                .ThenInclude(d => d.TestType)
            .FirstOrDefaultAsync(o => o.OrderID == id);

        if (order == null)
        {
            return NotFound();
        }

        return order;
    }

    [Authorize(Roles = "Admin,LabTechnician")]
    [HttpPut("order-details/{id}/result")]
    public async Task<IActionResult> UpdateLabResult(int id, UpdateLabResultRequest request)
    {
        var orderDetail = await _context.LabOrderDetails
            .Include(d => d.LabOrder)
            .FirstOrDefaultAsync(d => d.OrderDetailID == id);

        if (orderDetail == null)
        {
            return NotFound();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        orderDetail.Result = request.Result;
        orderDetail.ResultDate = DateTime.UtcNow;
        orderDetail.PerformedByID = userId;
        orderDetail.Status = "Completed";

        if (!string.IsNullOrEmpty(request.ReferenceRange))
        {
            orderDetail.ReferenceRange = request.ReferenceRange;
        }

        // Check if all details are completed
        var allDetailsCompleted = await _context.LabOrderDetails
            .Where(d => d.OrderID == orderDetail.OrderID)
            .AllAsync(d => d.Status == "Completed");

        if (allDetailsCompleted)
        {
            orderDetail.LabOrder.Status = "Completed";
        }
        else
        {
            orderDetail.LabOrder.Status = "InProgress";
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
}