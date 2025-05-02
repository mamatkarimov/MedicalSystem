using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Shared.DTOs.Auth
{
    public class TokenRevokeRequest : TokenRequest
    {
        public RevocationReason Reason { get; set; } = RevocationReason.UserAction;
    }

    public enum RevocationReason
    {
        UserAction,
        SecurityConcern,
        DeviceDeactivation
    }
}
