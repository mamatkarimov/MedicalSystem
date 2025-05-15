namespace MedicalSystem.Web.Models
{
    public class AppointmentDto
    {
        public Guid Id { get; set; }
        public string DoctorName { get; set; } = "";
        public DateTime Date { get; set; }
        public string Symptoms { get; set; } = "";
        public string Status { get; set; } = "";
    }
}
