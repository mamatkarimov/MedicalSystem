using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Shared.DTOs.User
{
    public class UserSessionDto
    {
        public string SessionId { get; set; }
        public string DeviceInfo { get; set; }
        public string IpAddress { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime? LastActivity { get; set; }
        public bool IsCurrent { get; set; }
    }
}
