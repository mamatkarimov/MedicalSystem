namespace AuthService.Shared.Enums
{
    /// <summary>
    /// Types of actions recorded in audit logs
    /// </summary>
    public enum AuditActionType
    {
        Create,
        Read,
        Update,
        Delete,
        Login,
        Logout,
        PasswordChange,
        RoleAssignment
    }

    
}