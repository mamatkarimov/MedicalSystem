using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.API.Models.Requests
{
public class CreateAppointmentRequest
{
    [Required]
    public int PatientID { get; set; }
    
    [Required]
    public string DoctorID { get; set; }
    
    [Required]
    public DateTime AppointmentDate { get; set; }
    
    public string Reason { get; set; }

}

public class AssignRoleRequest
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string RoleName { get; set; }
    }

    public class RegisterRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }

    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class CreateInstrumentalStudyRequest
    {
        [Required]
        public int PatientID { get; set; }
        
        [Required]
        public string StudyType { get; set; }
        
        public string Notes { get; set; }
    }

    public class UpdateInstrumentalStudyResultRequest
    {
        [Required]
        public string Results { get; set; }
        
        [Required]
        public string Conclusion { get; set; }
    }

    public class CreateLabOrderRequest
{
    [Required]
    public int PatientID { get; set; }
    
    [Required]
    public List<int> TestTypeIDs { get; set; }
    
    public string Priority { get; set; } = "Routine";
    public string Notes { get; set; }
}

public class UpdateLabResultRequest
{
    [Required]
    public string Result { get; set; }
    public string ReferenceRange { get; set; }
}

public class AddMedicalHistoryRequest
{
    [Required]
    public int PatientID { get; set; }
    
    [Required]
    public string HistoryType { get; set; }
    
    [Required]
    public string Description { get; set; }
}

public class CreatePrescriptionRequest
{
    [Required]
    public int PatientID { get; set; }
    
    [Required]
    public string Medication { get; set; }
    
    public string Dosage { get; set; }
    public string Frequency { get; set; }
    public string Duration { get; set; }
    public string Instructions { get; set; }
}

 public class AddPatientDocumentRequest
    {
        [Required]
        public int PatientID { get; set; }
        
        [Required]
        public string DocumentType { get; set; }
        
        [Required]
        public string DocumentNumber { get; set; }
        
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string IssuedBy { get; set; }
    }

    public class UpdatePatientDocumentRequest
    {
        [Required]
        public string DocumentType { get; set; }
        
        [Required]
        public string DocumentNumber { get; set; }
        
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string IssuedBy { get; set; }
    }
 public class AddToQueueRequest
    {
        [Required]
        public int PatientID { get; set; }
        
        public int? AppointmentID { get; set; }
        public int Priority { get; set; } = 5;
        public int? DepartmentID { get; set; }
        public string Notes { get; set; }
    }

    public class QueueStatusUpdateRequest
    {
        [Required]
        public string Status { get; set; } // InProgress, Completed, Cancelled
    }

    public class CreateInvoiceRequest
{
    [Required]
    public int PatientID { get; set; }
    
    public List<InvoiceItem> Items { get; set; }
    
    public string PaymentMethod { get; set; }
}

public class InvoiceItem
{
    [Required]
    public int ServiceID { get; set; }
    
    [Required]
    public int Quantity { get; set; } = 1;
    
    public decimal Discount { get; set; } = 0;
}

public class ProcessPaymentRequest
{
    [Required]
    public int InvoiceID { get; set; }
    
    [Required]
    public decimal Amount { get; set; }
    
    [Required]
    public string PaymentMethod { get; set; }
    
    public string Notes { get; set; }
}

public class RegisterPatientRequest
{
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    public string MiddleName { get; set; }
    
    [Required]
    public DateTime BirthDate { get; set; }
    
    [Required]
    public char Gender { get; set; }
    
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string InsuranceNumber { get; set; }
    public string InsuranceCompany { get; set; }
    
    // For self-registration
    public string Username { get; set; }
    public string Password { get; set; }
} 
public class AdmitPatientRequest
{
    [Required]
    public int PatientID { get; set; }
    
    [Required]
    public int BedID { get; set; }
    
    [Required]
    public string DiagnosisOnAdmission { get; set; }
    
    [Required]
    public string AttendingDoctorID { get; set; }
}

public class DischargePatientRequest
{
    [Required]
    public string DiagnosisOnDischarge { get; set; }
}
}