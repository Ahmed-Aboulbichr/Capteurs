using AutoMapper;
using Capteurs.API.Models.Domain;
using Capteurs.API.Models.DTOs;

namespace Capteurs.API.Mapper
{
    public class CapteursProfile : Profile
    {

        public CapteursProfile()
        {
            CreateMap<Capteur, CapteurDto>()
                .ReverseMap();

            CreateMap<User, UserDto>()
                .ReverseMap();
        }
    }
}
