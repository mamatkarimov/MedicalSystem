using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Application.Models.Requests
{
    public class AssignRoleRequest
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}