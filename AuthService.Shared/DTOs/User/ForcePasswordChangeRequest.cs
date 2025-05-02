namespace AuthService.Shared.DTOs.User
{
    public class ForcePasswordChangeRequest
    {
        public string UserId { get; set; }
        public bool ForcePasswordChange { get; set; }
    }
}