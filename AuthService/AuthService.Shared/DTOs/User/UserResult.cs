using AuthService.Shared.DTOs.User;
using AuthService.Shared.DTOs;

namespace AuthService.Core.Models.Responses
{
    public class UserResult : BaseResponse
    {
        public UserDto User { get; set; }
    }
}