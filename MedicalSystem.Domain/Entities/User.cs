using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace MedicalSystem.Domain.Entities
{    
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;        
        public string Email { get; set; } = default!;
        public bool IsActive { get; set; } = true;
        //public string Role { get; set; } = default!;
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public StaffProfile? StaffProfile { get; set; } // Optional: if the user is a staff member
    }

}