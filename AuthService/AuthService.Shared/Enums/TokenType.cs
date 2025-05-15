namespace AuthService.Shared.Enums
{
    /// <summary>
    /// Different types of JWT tokens used in the system
    /// </summary>
    public enum TokenType
    {
        /// <summary> Standard access token </summary>
        Access,

        /// <summary> Refresh token </summary>
        Refresh,

        /// <summary> Email verification token </summary>
        EmailVerification,

        /// <summary> Password reset token </summary>
        PasswordReset,

        /// <summary> Two-factor authentication token </summary>
        TwoFactor
    }
}