using AutoMapper;
using Shopping.Dtos.RequestDtos;
using Shopping.Models;

namespace Shopping.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SignUpDto, User>();
        }
    }
}
