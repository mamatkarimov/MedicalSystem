namespace AuthService.Shared.Enums
{
    /// <summary>
    /// Available two-factor authentication providers
    /// </summary>
    public enum TwoFactorProvider
    {
        /// <summary> Authentication app (TOTP) </summary>
        Authenticator,

        /// <summary> SMS verification </summary>
        SMS,

        /// <summary> Email verification </summary>
        Email,

        /// <summary> Hardware security key </summary>
        SecurityKey
    }
}