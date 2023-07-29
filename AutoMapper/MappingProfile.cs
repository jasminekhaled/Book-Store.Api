using AutoMapper;
using Shopping.Dtos.RequestDtos;
using Shopping.Dtos.ResponseDtos;
using Shopping.Models;

namespace Shopping.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SignUpDto, User>()
                .ForMember(src => src.Password, opt => opt.Ignore());
            CreateMap<SignUpDto, UserDto>();
        }
    }
}
