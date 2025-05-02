using AuthService.Core.Interfaces;
using AuthService.Shared.DTOs;

namespace AuthService.Shared.DTOs.User
{
    public class PasswordChangeResult : BaseResponse
    {
        public bool PasswordChanged { get; set; }
        public DateTimeOffset? PasswordExpirationDate { get; set; }
    }

    // Result DTOs
    public class UserResult : BaseResponse
    {
        public UserDto User { get; set; }
    }

    public class UserListResult : BaseResponse
    {
        public IEnumerable<UserDto> Users { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class DeleteUserResult : BaseResponse
    {
        public bool SoftDeleted { get; set; }
        public DateTimeOffset? DeletionDate { get; set; }
    }
   
    public class ProfileResult : BaseResponse
    {
        public ProfileDto Profile { get; set; }
    }

   

    public class UserRolesResult : BaseResponse
    {
        public IEnumerable<string> Roles { get; set; }
    }

    



    

  

    

    

    public class LoginAuditResult : BaseResponse
    {
        public IEnumerable<LoginRecordDto> Logins { get; set; }
        public int TotalCount { get; set; }
    }

    public class SecurityInfoResult : BaseResponse
    {
        public DateTimeOffset LastPasswordChangeDate { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public int ActiveSessions { get; set; }
        public int RegisteredDevices { get; set; }
    }
   
}