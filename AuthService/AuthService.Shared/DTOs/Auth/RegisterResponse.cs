namespace AuthService.Shared.DTOs.Auth
{
    public class RegisterResponse
    {
        public bool Success { get; set; }
        public string UserId { get; set; }
        public bool EmailVerificationRequired { get; set; }
        public string EmailVerificationToken { get; set; } // Only returned if email verification needed
    }

   
}
