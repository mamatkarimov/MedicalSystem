using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Shared.DTOs.Auth
{
    // AuthService.Shared/DTOs/Auth/TokenValidationRequest.cs
    public class TokenValidationRequest
    {
        [Required]
        public string Token { get; set; }
    }

    // AuthService.Shared/DTOs/Auth/ImpersonationRequest.cs
    public class ImpersonationRequest
    {
        [Required]
        public string UserId { get; set; }
    }

   
}
