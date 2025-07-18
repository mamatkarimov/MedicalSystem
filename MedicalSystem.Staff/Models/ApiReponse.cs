
namespace MedicalSystem.Staff.Models;

public class ApiResponse<T>
{
    public T Result { get; set; }
    public List<string> Errors { get; set; }
    public static async Task<ApiResponse<T>> HandleExceptionAsync(Func<Task<ApiResponse<T>>> action)
    {
        try
        {
            var result = await action();
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new ApiResponse<T>
            {
                Errors = new List<string> {e.Message}
            };
        }
    }
}


public class AuthRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}


public class AuthResponse
{
    public string JwtToken { get; set; }
    public string RefreshToken { get; set; }
}

//public record AuthResponse
//{
//    public string? UserId { get; set; }
//    public string? Username { get; set; }
//    public string? Email { get; set; }
//    public List<string>? Roles { get; set; }
//    public string? Token { get; set; }
//}