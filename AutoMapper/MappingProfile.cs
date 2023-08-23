using AutoMapper;
using Shopping.Dtos.BookDtos.RequestDtos;
using Shopping.Dtos.BookDtos.ResponseDtos;
using Shopping.Dtos.CartsDtos.ResponseDtos;
using Shopping.Dtos.RequestDtos;
using Shopping.Dtos.ResponseDtos;
using Shopping.Models.AuthModule;
using Shopping.Models.BookModule;

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
            CreateMap<BookRequestDto, Book>()
                .ForMember(src => src.bookCategories, opt => opt.Ignore())
                .ForMember(src => src.Poster, opt => opt.Ignore());
            CreateMap<Book, CartDto>()
               .ForMember(src => src.WantedCopies, opt => opt.Ignore())
               .ForMember(src => src.Price, opt => opt.Ignore());


        }
    }
}
