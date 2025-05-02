namespace AuthService.Shared.DTOs.User
{
    public class PermissionResult : BaseResponse
    {
        public int PermissionsChanged { get; set; }
        public UserPermissionsResult CurrentPermissions { get; set; }
    }
}