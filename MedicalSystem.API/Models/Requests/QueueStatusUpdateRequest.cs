using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.API.Models.Requests
{
    public class QueueStatusUpdateRequest
    {
        [Required]
        public string Status { get; set; } // InProgress, Completed, Cancelled
    }
}