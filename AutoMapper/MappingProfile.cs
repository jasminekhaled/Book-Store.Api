using AutoMapper;
using Shopping.Dtos.BookDtos.RequestDtos;
using Shopping.Dtos.BookDtos.ResponseDtos;
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
            CreateMap<Category, CategoryDto>();
            CreateMap<AddCategoryDto, CategoryDto>();
            CreateMap<AddCategoryDto, Category>();
            CreateMap<Book, BookDto>();
        }
    }
}
