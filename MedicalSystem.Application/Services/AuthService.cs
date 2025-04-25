using MedicalSystem.Application.DTOs;
using MedicalSystem.Application.Interfaces;
using MedicalSystem.Domain.Interfaces;

namespace MedicalSystem.Application.Services;

// public class AuthService : IAuthService
// {
//     private readonly IUserRepository _userRepository;

//     public AuthService(IUserRepository userRepository)
//     {
//         _userRepository = userRepository;
//     }

//     public async Task<string> RegisterAsync(RegisterDto dto)
//     {
//         // Проверка, хеширование пароля и сохранение нового пользователя
//         var user = new User(dto.Username, dto.Email, dto.Password); // упрощённо
//         await _userRepository.AddAsync(user);
//         return "Пользователь успешно зарегистрирован";
//     }
// }
