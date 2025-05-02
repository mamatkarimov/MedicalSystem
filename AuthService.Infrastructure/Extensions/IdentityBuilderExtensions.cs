using AuthService.Core.Entities;
using AuthService.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Infrastructure.Extensions
{
    public static class IdentityBuilderExtensions
    {
        public static IdentityBuilder AddApplicationIdentity(this IdentityBuilder builder)
        {
            builder
                .AddUserStore<ApplicationUserStore>()
                .AddRoleStore<ApplicationRoleStore>()
                .AddDefaultTokenProviders();

            builder.Services.AddTransient<IUserStore<ApplicationUser>, ApplicationUserStore>();
            builder.Services.AddTransient<IRoleStore<ApplicationRole>, ApplicationRoleStore>();
            builder.Services.AddScoped<IPasswordHasher<ApplicationUser>, CustomPasswordHasher>();

            return builder;
        }

        public static IdentityBuilder AddCustomTokenProviders(this IdentityBuilder builder)
        {
            builder.Services.AddTokenProvider<EmailConfirmationTokenProvider<ApplicationUser>>("emailconfirmation");
            builder.Services.AddTokenProvider<PhoneConfirmationTokenProvider<ApplicationUser>>("phoneconfirmation");
            return builder;
        }
    }
}