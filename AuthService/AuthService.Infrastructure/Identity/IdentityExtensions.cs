using AuthService.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Identity
{
    public static class IdentityExtensions
    {
        public static async Task<IdentityResult> AddPermissionClaimAsync(
            this RoleManager<ApplicationRole> roleManager,
            ApplicationRole role,
            string permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            if (!allClaims.Any(c => c.Type == "Permission" && c.Value == permission))
            {
                return await roleManager.AddClaimAsync(role,
                    new Claim("Permission", permission));
            }
            return IdentityResult.Success;
        }

        public static async Task<bool> HasPermissionAsync(
            this UserManager<User> userManager,
            User user,
            string permission)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            if (userClaims.Any(c => c.Type == "Permission" && c.Value == permission))
                return true;

            var roles = await userManager.GetRolesAsync(user);
            foreach (var roleName in roles)
            {
                var role = await userManager.FindByNameAsync(roleName);
                var roleClaims = await userManager.GetClaimsAsync(role);
                if (roleClaims.Any(c => c.Type == "Permission" && c.Value == permission))
                    return true;
            }

            return false;
        }
    }
}