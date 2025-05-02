namespace AuthService.Shared.Enums
{
    /// <summary>
    /// Defines the scope of permissions in the system
    /// </summary>
    public enum PermissionScope
    {
        /// <summary> Read-only access </summary>
        Read,

        /// <summary> Create new resources </summary>
        Create,

        /// <summary> Modify existing resources </summary>
        Update,

        /// <summary> Delete resources </summary>
        Delete,

        /// <summary> Full administrative access </summary>
        Admin
    }
}