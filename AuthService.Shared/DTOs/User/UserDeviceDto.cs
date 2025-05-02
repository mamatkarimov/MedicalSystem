using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Shared.DTOs.User
{
    public class UserDeviceDto
    {
        public Guid Id { get; set; }
        public string DeviceName { get; set; }
        public string Platform { get; set; }
        public string OsVersion { get; set; }
        public DateTime LastAccessed { get; set; }
        public string IpAddress { get; set; }
        public bool IsTrusted { get; set; }
    }
}
