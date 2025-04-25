using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MedicalSystem.API.Models;
using MedicalSystem.API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using MedicalSystem.API.Data;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystem.API.Controllers
{
[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = _userManager.Users.Select(u => new
        {
            u.Id,
            u.UserName,
            u.Email,
            u.FirstName,
            u.LastName,
            u.MiddleName,
            Roles = _userManager.GetRolesAsync(u).Result
        }).ToList();

        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        var roles = await _userManager.GetRolesAsync(user);

        return Ok(new
        {
            user.Id,
            user.UserName,
            user.Email,
            user.FirstName,
            user.LastName,
            user.MiddleName,
            Roles = roles
        });
    }

    [HttpPost("roles")]
    public async Task<IActionResult> CreateRole([FromBody] string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return BadRequest("Role name is required");

        var roleExists = await _roleManager.RoleExistsAsync(roleName);
        if (roleExists)
            return BadRequest("Role already exists");

        var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok("Role created successfully");
    }

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
            return NotFound("User not found");

        var roleExists = await _roleManager.RoleExistsAsync(request.RoleName);
        if (!roleExists)
            return NotFound("Role not found");

        var result = await _userManager.AddToRoleAsync(user, request.RoleName);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok("Role assigned successfully");
    }

   
    }
}