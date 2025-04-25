namespace MedicalSystemAPI.Mapping
{
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterRequest, RegisterDto>();
    }
}
}