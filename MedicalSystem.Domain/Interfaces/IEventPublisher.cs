using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Domain.Interfaces
{
    public interface IEventPublisher
    {
        Task Publish<TEvent>(TEvent @event) where TEvent : class;
    }

    public interface IEventHandler<in TEvent> where TEvent : class
    {
        Task Handle(TEvent @event);
    }
}
