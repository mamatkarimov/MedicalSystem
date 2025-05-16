using System.Text.Json.Serialization;

namespace MedicalSystem.API
{
    public class RealmAccess
    {
        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; } = new List<string>();
    }
}
