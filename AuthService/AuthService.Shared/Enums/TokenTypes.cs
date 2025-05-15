namespace AuthService.Shared.Enums
{
    public class TokenTypes
    {
        public const string AccessToken = "access";
        public const string RefreshToken = "refresh";
        public const string PasswordReset = "pwd_reset";
        public const string EmailConfirmation = "email_conf";
        public const string TwoFactor = "2fa";
    }
}