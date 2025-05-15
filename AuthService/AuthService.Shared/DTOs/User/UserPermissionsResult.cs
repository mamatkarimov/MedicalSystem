namespace AuthService.Shared.DTOs.User
{
    public class UserPermissionsResult : BaseResponse
    {
        public IEnumerable<string> Permissions { get; set; }
    }

    
}