using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalSystem.Domain.Entities;

public partial class DbUser
{
    //[Key]
    //[Column("UserID")]
    //public int UserId { get; set; }

    //[Column("PersonID")]
    //public long PersonId { get; set; }

    //[StringLength(50)]
    //public string UserName { get; set; } = null!;

    //[MaxLength(4000)]
    //public byte[] UserPassword { get; set; } = null!;

    //[Column("PersonTypeID")]
    //public short PersonTypeId { get; set; }

    //[InverseProperty("DbUser")]
    //public virtual ICollection<DepUser> DepUsers { get; } = new List<DepUser>();

    //[InverseProperty("DbUser")]
    //public virtual ICollection<Issued> Issueds { get; } = new List<Issued>();

    //[InverseProperty("DbUser")]
    //public virtual ICollection<Patient> Patients { get; } = new List<Patient>();

    //[ForeignKey("PersonId")]
    //[InverseProperty("ApplicationUsers")]
    //public virtual Person Person { get; set; } = null!;

    //[InverseProperty("DbUser")]
    //public virtual ICollection<PharmDocItem> PharmDocItems { get; } = new List<PharmDocItem>();

    //[InverseProperty("DbUser")]
    //public virtual ICollection<UsersRole> UsersRoles { get; } = new List<UsersRole>();

    //[ForeignKey("UserId")]
    //[InverseProperty("DbUsers")]
    //public virtual ICollection<Service> Services { get; } = new List<Service>();
}
