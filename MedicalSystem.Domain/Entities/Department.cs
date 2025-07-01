using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Domain.Entities
{
   
    public class Department
    {

        [Key]
        public int DepartmentID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public string HeadDoctorID { get; set; }

        // Navigation properties
        public User HeadDoctor { get; set; }
        public ICollection<Ward> Wards { get; set; }
    }


}