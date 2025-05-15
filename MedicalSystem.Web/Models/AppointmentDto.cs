namespace MedicalSystem.Web.Models
{
    public class AppointmentDto
    {
        public Guid Id { get; set; }
        public string Doctor { get; set; } = "";
        public DateTime Date { get; set; }
        public string Symptoms { get; set; } = "";
        public string Status { get; set; } = "";
    }
}
