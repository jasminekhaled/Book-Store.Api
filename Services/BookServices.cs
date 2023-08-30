using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shopping.Context;
using Shopping.Dtos;
using Shopping.Dtos.BookDtos.RequestDtos;
using Shopping.Dtos.BookDtos.ResponseDtos;
using Shopping.Dtos.ResponseDtos;
using Shopping.Interfaces;
using Shopping.Models.BookModule;

namespace Shopping.Services
{
    public class BookServices : IBookServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public BookServices(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
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
                if (await _context.Categories.AnyAsync(c => c.Name == dto.Name))
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

                return new GeneralResponse<CategoryDto>
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


        public async Task<GeneralResponse<BookDto>> AddBook([FromForm] BookRequestDto dto)
        {
            try
            {
                var MaxAllowedPosterSize = _configuration.GetValue<long>("MaxAllowedPosterSize");
                List<string> AllowedExtenstions = _configuration.GetSection("AllowedExtenstions").Get<List<string>>();

                if (!AllowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                {
                    return new GeneralResponse<BookDto>
                    {
                        IsSuccess = false,
                        Message = "Only .jpg and .png Images Are Allowed."
                    };
                }

                if (dto.Poster.Length > MaxAllowedPosterSize)
                {
                    return new GeneralResponse<BookDto>
                    {
                        IsSuccess = false,
                        Message = "Max Allowed Size Is 1MB."
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
                if (dto.Rate < 0 || dto.Rate > 10)
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
                foreach(var id in dto.CategoryId)
                {
                    if (!await _context.Categories.AnyAsync(i => i.Id == id))
                    {
                       return new GeneralResponse<BookDto>
                    {
                        IsSuccess = false,
                        Message = "No Category exists with this Id."
                    }; 
                    }
                }
                using var dataStream = new MemoryStream();
                await dto.Poster.CopyToAsync(dataStream);

                var book = _mapper.Map<Book>(dto);
                 
                book.Poster = dataStream.ToArray();

                await _context.Books.AddAsync(book);
                _context.SaveChanges();
                 
                var bookCategories = dto.CategoryId.Select(c => new BookCategories
                {
                    bookId = book.Id,
                    categoryId = c
                }).ToList();

                await _context.bookCategories.AddRangeAsync(bookCategories); 
                _context.SaveChanges();

                var names = await _context.bookCategories.Where(c => c.bookId == book.Id).Select(bc => bc.Category.Name).ToListAsync();

                var data = _mapper.Map<BookDto>(book);
                data.categoryName = names;
                data.categoryId = dto.CategoryId;

                return new GeneralResponse<BookDto>
                {
                    IsSuccess = true,
                    Message = "The Book is added successfully.",
                    Data = data

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



        public async Task<GeneralResponse<BookDto>> BookDetails(int id)
        {
            try 
            {
                var book = await _context.Books.FindAsync(id);
                if(book == null)
                {
                    return new GeneralResponse<BookDto>
                    {
                        IsSuccess = false,
                        Message = "No book is existed with this id."
                    };
                }
                var names = await _context.bookCategories.Where(c => c.bookId == book.Id).Select(bc => bc.Category.Name).ToListAsync();
                var ids = await _context.bookCategories.Where(c => c.bookId == book.Id).Select(i => i.categoryId).ToListAsync();
                
                var data = _mapper.Map<BookDto>(book);
                data.categoryName = names;
                data.categoryId = ids;

                return new GeneralResponse<BookDto>
                {
                    IsSuccess = true,
                    Message = "Here are the Details.",
                    Data = data
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



        public async Task<GeneralResponse<BookDto>> EditBook(int id, [FromForm]EditRequestDto dto)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    return new GeneralResponse<BookDto>
                    {
                        IsSuccess = false,
                        Message = "No book is existed with this id."
                    };
                }
                if(dto.Year != null && dto.Year < 1)
                {
                    return new GeneralResponse<BookDto>
                    {
                        IsSuccess = false,
                        Message = "The Year must be a positive number."
                    };
                }
                if (dto.NumOfCopies != null && dto.NumOfCopies < 1)
                {
                    return new GeneralResponse<BookDto>
                    {
                        IsSuccess = false,
                        Message = "Num of copies should be at least 1."
                    };
                }
                if (dto.Price != null && dto.Price < 0)
                {
                    return new GeneralResponse<BookDto>
                    {
                        IsSuccess = false,
                        Message = "The Price cannot be a negative number."
                    };
                }
                if (dto.Rate != null)
                {
                    if (dto.Rate < 0 || dto.Rate > 10)
                    {
                        return new GeneralResponse<BookDto>
                        {
                            IsSuccess = false,
                            Message = "The Rate must be between 0 and 10."
                        };
                    }
                }
                if(dto.Poster != null)
                {
                    var MaxAllowedPosterSize = _configuration.GetValue<long>("MaxAllowedPosterSize");
                    List<string> AllowedExtenstions = _configuration.GetSection("AllowedExtenstions").Get<List<string>>();

                    if (!AllowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    {
                        return new GeneralResponse<BookDto>
                        {
                            IsSuccess = false,
                            Message = "Only .jpg and .png Images Are Allowed."
                        };
                    }

                    if (dto.Poster.Length > MaxAllowedPosterSize)
                    {
                        return new GeneralResponse<BookDto>
                        {
                            IsSuccess = false,
                            Message = "Max Allowed Size Is 1MB."
                        };
                    }
                }

                if(dto.CategoryId != null)
                {
                    foreach (var iD in dto.CategoryId)
                    {
                        if (!await _context.Categories.AnyAsync(i => i.Id == iD))
                        {
                            return new GeneralResponse<BookDto>
                            {
                                IsSuccess = false,
                                Message = "No Category exists with this Id."
                            };
                        }
                    }
                    var existingBookCategories = await _context.bookCategories
                     .Where(bc => bc.bookId == book.Id)
                     .ToListAsync();

                    _context.bookCategories.RemoveRange(existingBookCategories);

                    var bookCategories = dto.CategoryId.Select(c => new BookCategories
                    {
                        bookId = book.Id,
                        categoryId = c
                    }).ToList();
 
                   await _context.bookCategories.AddRangeAsync(bookCategories);
                   _context.Books.UpdateRange(book);
                   _context.SaveChanges(); 

                   
                }


                if(dto.Poster!=null)
                {
                    using var dataStream = new MemoryStream();
                    await dto.Poster.CopyToAsync(dataStream);
                    book.Poster = dataStream.ToArray();
                }

                book.Title = dto.Title ?? book.Title;
                book.Author = dto.Author ?? book.Author;
                book.Description = dto.Description ?? book.Description;
                book.NumOfCopies = dto.NumOfCopies ?? book.NumOfCopies;
                book.Price = dto.Price ?? book.Price;
                book.Year = dto.Year ?? book.Year;
                book.Rate = dto.Rate ?? book.Rate;

                _context.Books.Update(book);
                _context.SaveChanges();

                var names = await _context.bookCategories.Where(c => c.bookId == book.Id).Select(bc => bc.Category.Name).ToListAsync();

                var data = _mapper.Map<BookDto>(book);
                data.categoryName = names;
                data.categoryId = dto.CategoryId;

                return new GeneralResponse<BookDto>
                {
                    IsSuccess = true,
                    Message = "The book is editted successfully.",
                    Data = data
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



        public async Task<GeneralResponse<BookDto>> BuyABook(int bookId,int userId,BuyABookRequestDto dto)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return new GeneralResponse<BookDto>
                    {
                        IsSuccess = false,
                        Message = "No user Found.",
                    };
                }

                var book = await _context.Books.FindAsync(bookId);
                if (book == null)
                {
                    return new GeneralResponse<BookDto>
                    {
                        IsSuccess = false,
                        Message = "No Book Found.",
                    };
                }

                if(dto.NumOfCopies < 1 || dto.NumOfCopies > book.NumOfCopies)
                {
                    return new GeneralResponse<BookDto>
                    {
                        IsSuccess = false,
                        Message = "the number of copies that you want isnot available.",
                    };
                }

                book.NumOfCopies = book.NumOfCopies - dto.NumOfCopies;
                
                var bookUsers = new BookUsers
                {
                    bookId = book.Id,
                    userId = user.id,
                    Date = DateTime.Now,
                    NumOfBoughtCopies = dto.NumOfCopies
                };
                await _context.bookUsers.AddRangeAsync(bookUsers);

                book.NumOfSoldCopies += dto.NumOfCopies; 


                _context.Books.Update(book);
                _context.SaveChanges();

                var names = await _context.bookCategories.Where(c => c.bookId == book.Id).Select(bc => bc.Category.Name).ToListAsync();
                var ids = await _context.bookCategories.Where(c => c.bookId == book.Id).Select(i => i.categoryId).ToListAsync();

                var data = _mapper.Map<BookDto>(book);
                data.categoryName = names;
                data.categoryId = ids;

                return new GeneralResponse<BookDto>
                {
                    IsSuccess = true,
                    Message = "The book has been successfully purchased.",
                    Data = data
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



        



    }
}
