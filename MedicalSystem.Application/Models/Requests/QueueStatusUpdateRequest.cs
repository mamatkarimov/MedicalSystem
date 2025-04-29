using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Application.Models.Requests
{
    public class QueueStatusUpdateRequest
    {
        [Required]
        public string Status { get; set; } // InProgress, Completed, Cancelled
    }
}