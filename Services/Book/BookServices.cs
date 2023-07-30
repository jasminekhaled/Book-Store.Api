using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Context;
using Shopping.Dtos;
using Shopping.Dtos.BookDtos.RequestDtos;
using Shopping.Dtos.BookDtos.ResponseDtos;
using Shopping.Dtos.ResponseDtos;
using Shopping.Models;

namespace Shopping.Services.Book
{
    public class BookServices : IBookServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private new List<string> _AllowedExtenstions = new List<string> { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1048576;
        public BookServices(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        public async Task<GeneralResponse<List<CategoryDto>>> GetAllCategories()
        {
            try
            {
                var list = await _context.Categories.ToListAsync();
                
                return new GeneralResponse<List<CategoryDto>>
                {
                    IsSuccess = true,
                    Message = "Categories Listed Successfully.",
                    Data = _mapper.Map<List<CategoryDto>>(list)
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<CategoryDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong.",
                    Error = ex
                };
            }


        }  


        public async Task<GeneralResponse<CategoryDto>> AddCategory(AddCategoryDto dto)
        {
            try
            {
                if(await _context.Categories.AnyAsync(c => c.Name == dto.Name))
                {
                    return new GeneralResponse<CategoryDto>
                    {
                        IsSuccess = false,
                        Message = "This Category is already Existed.",
                    };
                }

                var category = _mapper.Map<Category>(dto);
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();

                return new  GeneralResponse<CategoryDto>
                {
                    IsSuccess = true,
                    Message = "New Category Is Added Successfully.",
                    Data = _mapper.Map<CategoryDto>(dto)
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<CategoryDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong.",
                    Error = ex
                };
            }

        }


        public async Task<GeneralResponse<CategoryDto>> DeleteCategory(int id)
        {
            try 
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    return new GeneralResponse<CategoryDto>
                    {
                        IsSuccess = false,
                        Message = "No Category Found.",
                    };
                }
                _context.Categories.Remove(category);
                _context.SaveChanges();

                return new GeneralResponse<CategoryDto>
                {
                    IsSuccess = true,
                    Message = "The Category is deleted."
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<CategoryDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong.",
                    Error = ex
                };
            }

        }


        public async Task<GeneralResponse<List<BookDto>>> GetAllBooks()
        {
            try
            {
                var list = await _context.Books.ToListAsync();

                return new GeneralResponse<List<BookDto>>
                {
                    IsSuccess = true,
                    Message = "Books Listed Successfully.",
                    Data = _mapper.Map<List<BookDto>>(list)
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<BookDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong.",
                    Error = ex
                };
            }
        }


        public async Task<GeneralResponse<BookDto>> AddBook([FromForm]BookRequestDto dto)
        {
            try 
            {
                if(!_AllowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                {
                    return new GeneralResponse<BookDto>
                    {
                        IsSuccess = false,
                        Message = "only .jpg and .png images are allowed."
                    };
                }

                if (dto.Poster.Length > _maxAllowedPosterSize)
                {
                    return new GeneralResponse<BookDto>
                    {
                        IsSuccess = false,
                        Message = "max allowed size is 1MB."
                    };
                }
                if (dto.NumOfCopies < 1)
                {
                    return new GeneralResponse<BookDto>
                    {
                        IsSuccess = false,
                        Message = "Num of copies should be at least 1."
                    };
                }
                if (dto.Price < 0)
                {
                    return new GeneralResponse<BookDto>
                    {
                        IsSuccess = false,
                        Message = "The Price cannot be a negative number."
                    };
                }
                if (dto.Rate < 0 && dto.Rate > 10)
                {
                    return new GeneralResponse<BookDto>
                    {
                        IsSuccess = false,
                        Message = "The Rate must be between 0 and 10."
                    };
                }
                if (dto.Year < 1)
                {
                    return new GeneralResponse<BookDto>
                    {
                        IsSuccess = false,
                        Message = "The Year must be a positive number."
                    };
                }


                return new GeneralResponse<BookDto>
                {
                    IsSuccess = true,
                    Message = "Books Listed Successfully."
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<BookDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong.",
                    Error = ex
                };
            }

        }


        public async Task<GeneralResponse<BookDto>> DeleteBook(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    return new GeneralResponse<BookDto>
                    {
                        IsSuccess = false,
                        Message = "No Book Found.",
                    };
                }
                _context.Books.Remove(book);
                _context.SaveChanges();

                return new GeneralResponse<BookDto>
                {
                    IsSuccess = true,
                    Message = "The Book is deleted."
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<BookDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong.",
                    Error = ex
                };
            }
        }



        public Task<GeneralResponse<BookDto>> BookDetails(BookRequestDto dto)
        {
            throw new NotImplementedException();
        }



        public Task<GeneralResponse<BookDto>> EditBook(BookRequestDto dto)
        {
            throw new NotImplementedException();
        }



        public Task<GeneralResponse<BookDto>> BuyABook(int id)
        {
            throw new NotImplementedException();
        }



        public Task<GeneralResponse<List<BookDto>>> GetBooksByUser(int id)
        {
            throw new NotImplementedException();
        }



    }
}
