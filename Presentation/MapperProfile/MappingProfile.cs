using AutoMapper;
using Entities.Models;
using Shared.DTOs.Auth;
using Shared.DTOs.Bicycle;
using Shared.DTOs.Document;
using Shared.DTOs.Reservation;
using Shared.DTOs.Station;
using Shared.DTOs.User;

namespace Presentation.MapperProfile;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AppUser, RegisterDto>().ReverseMap();
        CreateMap<AppUser, AppUserDto>().ReverseMap();

        CreateMap<Station, StationDto>().ReverseMap();
        CreateMap<Station, StationForCreationDto>().ReverseMap();
        CreateMap<Station, StationForUpdationDto>().ReverseMap();

        CreateMap<Bicycle, BicycleDto>().ReverseMap();
        CreateMap<Bicycle, BicycleForCreationDto>().ReverseMap();
        CreateMap<Bicycle, BicycleForUpdationDto>().ReverseMap();

        CreateMap<Document, DocumentDto>().ReverseMap();
        CreateMap<Document, DocumentForVerificationDto>().ReverseMap();

        CreateMap<Reservation, ReservationDto>().ReverseMap();
    }
}
