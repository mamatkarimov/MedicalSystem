using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MedicalSystem.Domain.Interfaces;
using MedicalSystem.Domain.Events;

namespace MedicalSystem.AuthService.Services
{
  
    // Supporting Interface
    public interface IUserEvent
    {
        string IdentityId { get; }
    }

}
