using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Domain.Enums
{
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string Doctor = "Doctor";
        public const string Nurse = "Nurse";
        public const string Reception = "Reception";
        public const string Cashier = "Cashier";
        public const string Lab = "Laboratory";
        public const string ChefDoctor = "ChefDoctor";
        public const string User = "User"; // Default
    }
}
