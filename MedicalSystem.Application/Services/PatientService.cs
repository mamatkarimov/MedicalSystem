// MedicalSystem.Core/Services/PatientService.cs
using AutoMapper;
using MedicalSystem.Application.Interfaces;
using MedicalSystem.Application.Models.Requests;
using MedicalSystem.Application.Models.Responses;
using MedicalSystem.Application.Models.Results;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Infrastructure.Data;
using MedicalSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
namespace MedicalSystem.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PatientService(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResult<PatientResponse>> SelfRegisterAsync(RegisterPatientRequest request)
        {
            // 1. Validate email uniqueness
            if (await _userManager.FindByEmailAsync(request.Email) != null)
                return ServiceResult<PatientResponse>.Failure("Email already registered");

            // 2. Create Identity user
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                PhoneNumber = request.Phone
            };

            var createResult = await _userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
                return ServiceResult<PatientResponse>.Failure(createResult.Errors.Select(e => e.Description));

            // 3. Assign Patient role
            await _userManager.AddToRoleAsync(user, Roles.Patient);

            // 4. Create Patient profile
            var patient = new Patient
            {
               // PatientID = user.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = request.BirthDate,
                // Map other properties
            };

            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();

            // 5. Return response
            return ServiceResult<PatientResponse>.Success(
                _mapper.Map<PatientResponse>(patient));
        }
    }
}