using AutoMapper;
using HotelListingAPI.Data;
using HotelListingAPI.Models.Country;
using HotelListingAPI.Models.Hotel;
using HotelListingAPI.Models.Users;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HotelListingAPI.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Country, CreateCountryDto>().ReverseMap();
            CreateMap<Country, GetCountryDro>().ReverseMap();
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Country, UpdateCountryDto>().ReverseMap();

            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<Hotel, CreateHotelDto>().ReverseMap();

            CreateMap<ApiUserDto, ApiUser>().ReverseMap();
        }
    }
}
