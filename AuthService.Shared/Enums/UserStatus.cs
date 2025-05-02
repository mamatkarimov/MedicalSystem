namespace AuthService.Shared.Enums
{
    /// <summary>
    /// Represents the possible statuses of a user account
    /// </summary>
    public enum UserStatus
    {
        /// <summary> Newly created, not yet activated </summary>
        PendingActivation,

        /// <summary> Active and fully functional </summary>
        Active,

        /// <summary> Temporarily locked due to security policy </summary>
        Locked,

        /// <summary> Permanently deactivated </summary>
        Deactivated,

        /// <summary> Marked for deletion (GDPR compliance) </summary>
        ScheduledForDeletion
    }
}