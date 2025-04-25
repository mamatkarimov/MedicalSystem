using System.ComponentModel.DataAnnotations;

namespace MedicalSystemAPI.Models.DTOs
{
public class AssignRoleRequest
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}