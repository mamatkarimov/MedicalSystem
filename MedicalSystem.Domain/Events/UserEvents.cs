using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Domain.Events
{
    public record UserCreatedEvent(
    string IdentityId,
    string Email,
    string FirstName,
    string LastName);

    public record UserUpdatedEvent(
        string IdentityId,
        string? NewEmail,
        string? NewFirstName,
        string? NewLastName);

    public record UserDeletedEvent(string IdentityId);
}
