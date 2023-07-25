using AutoMapper;
using Shopping.Dtos;
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
