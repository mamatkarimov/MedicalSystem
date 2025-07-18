using System.Collections.Generic;

namespace MedicalSystem.Application.DTOs
{
    // DTOs/LoginResponse.cs
    public class LoginResponse
    {
        public string Token { get; set; }

        public List<string> Roles { get; set; } = new List<string>();
    }
}
