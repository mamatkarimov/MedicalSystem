namespace AuthService.Shared.Enums
{
    /// <summary>
    /// Standardized error codes for API responses
    /// </summary>
    public enum ApiErrorCode
    {
        // General errors (0-99)
        UnknownError = 0,
        ValidationFailed = 1,
        ResourceNotFound = 2,

        // Authentication errors (100-199)
        InvalidCredentials = 100,
        AccountLocked = 101,
        TokenExpired = 102,
        InvalidToken = 103,

        // Authorization errors (200-299)
        PermissionDenied = 200,
        RoleRestricted = 201,

        // User management errors (300-399)
        EmailAlreadyExists = 300,
        WeakPassword = 301
    }
}