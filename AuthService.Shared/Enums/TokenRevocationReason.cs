namespace AuthService.Shared.Enums
{
    /// <summary>
    /// Reasons for revoking an access token
    /// </summary>
    public enum TokenRevocationReason
    {
        /// <summary> User-initiated logout </summary>
        UserRequest,

        /// <summary> Security policy enforcement </summary>
        SecurityPolicy,

        /// <summary> Suspicious activity detected </summary>
        SuspiciousActivity,

        /// <summary> Password was changed </summary>
        PasswordChange,

        /// <summary> Device was deactivated </summary>
        DeviceDeactivation
    }
}