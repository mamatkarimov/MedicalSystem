using AutoMapper;
using MedicalSystem.Application.Models.Requests;
using MedicalSystem.Application.Models.Responses;
using MedicalSystem.Domain.Entities;

namespace MedicalSystem.API.Mapping
{
public class MappingProfile : Profile
{
        public MappingProfile()
        {
            CreateMap<Patient, PatientResponse>()
                .ForMember(dest => dest.Appointments,
                           opt => opt.MapFrom(src => src.Appointments));

            CreateMap<Appointment, AppointmentResponse>();
        }
    }