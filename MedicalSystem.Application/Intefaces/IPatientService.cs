// MedicalSystem.Core/Services/IPatientService.cs
using MedicalSystem.Application.Models.Requests;
using MedicalSystem.Application.Models.Responses;
using MedicalSystem.Application.Models.Results;
using System.Threading.Tasks;
namespace MedicalSystem.Application.Interfaces
{
    public interface IPatientService
    {
        Task<ServiceResult<PatientResponse>> SelfRegisterAsync(RegisterPatientRequest request);
        // Other patient-related methods
    }
}