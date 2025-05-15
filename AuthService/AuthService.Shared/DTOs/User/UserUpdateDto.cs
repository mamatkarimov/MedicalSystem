using System.ComponentModel.DataAnnotations;

namespace AuthService.Shared.DTOs.User
{
    public class UserUpdateDto
    {
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Url]
        public string ProfilePictureUrl { get; set; }
    }
}