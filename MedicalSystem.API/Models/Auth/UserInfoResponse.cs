using MedicalSystem.Domain.Entities;

namespace MedicalSystem.API.Models.Auth
{
    public class UserInfoResponse
    {
        public string Username { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
