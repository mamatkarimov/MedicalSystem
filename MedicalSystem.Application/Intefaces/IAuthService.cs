using System.Threading.Tasks;
using MedicalSystem.Application.DTOs;

namespace MedicalSystem.Application.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterDto dto);
}