using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shopping.Context;
using Shopping.Dtos;
using Shopping.Dtos.BookDtos.ResponseDtos;
using Shopping.Dtos.StatisticsDtos.ResponseDtos;
using Shopping.Interfaces;
using Shopping.Models.AuthModule;
using System;
using System.Collections.Generic;
using System.Linq;
using static Azure.Core.HttpHeader;

namespace Shopping.Services
{
    public class StatisticsServices : IStatisticsServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public StatisticsServices(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<GeneralResponse<List<TopRatedDto>>> TopRatedBooks()
        {
            try
            {
                var books = await _context.Books.OrderByDescending(r => r.Rate).Take(5).ToListAsync();
                var Data = _mapper.Map<List<TopRatedDto>>(books);
                foreach (var book in books)
                {
                    var names = await _context.bookCategories.Where(i => i.bookId == book.Id).Select(s => s.Category.Name).ToListAsync();
                    var data = Data.FirstOrDefault(i => i.id == book.Id);
                    data.Categories = names;
                }
                return new GeneralResponse<List<TopRatedDto>>
                {
                    IsSuccess = true,
                    Message = "Top Rated Books Listed Successfully.",
                    Data = Data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<TopRatedDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong.",
                    Error = ex
                };
            }

        }

        public async Task<GeneralResponse<List<MostSoldBooksDto>>> MostSoldBooksInGeneral()
        {
            try
            {
                var books = await _context.Books.OrderByDescending(s => s.NumOfSoldCopies).Take(5).ToListAsync();
                var Data = _mapper.Map<List<MostSoldBooksDto>>(books);
                foreach (var book in books)
                {
                    var names = await _context.bookCategories.Where(i => i.bookId == book.Id).Select(s => s.Category.Name).ToListAsync();
                    var data = Data.FirstOrDefault(i => i.id == book.Id);
                    data.Categories = names;
                }
                return new GeneralResponse<List<MostSoldBooksDto>>
                {
                    IsSuccess = true,
                    Message = "Most Sold Books Listed Successfully.",
                    Data = Data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<MostSoldBooksDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong.",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<List<MostSoldBooksDto>>> MostSoldBooksInPeriod(DateTime startDate, DateTime endDate)
        {
            try
            {
                var info = await _context.bookUsers
                    .Where(d => d.Date >= startDate && d.Date < endDate)
                    .GroupBy(g => g.bookId)
                    .Select(s => new
                    {
                        bookId = s.Key,
                        NumOfBoughtCopies = s.Sum(g => g.NumOfBoughtCopies)
                    })
                    .OrderByDescending(g => g.NumOfBoughtCopies)
                    .Take(5)
                    .ToListAsync();

                var ids = info.Select(s => s.bookId).ToList();
                var copies = info.Select(s => s.NumOfBoughtCopies).ToList();

                var books = await _context.Books
                    .Where(b => ids.Contains(b.Id))
                    .ToListAsync();


                var Data = _mapper.Map<List<MostSoldBooksDto>>(books);
                foreach (var book in books)
                {
                    var names = await _context.bookCategories.Where(i => i.bookId == book.Id).Select(s => s.Category.Name).ToListAsync();
                    var data = Data.FirstOrDefault(i => i.id == book.Id);
                    data.Categories = names;
                    data.NumOfSoldCopies = info.Where(i => i.bookId == book.Id).Select(s => s.NumOfBoughtCopies).FirstOrDefault(); 
                }
                
                return new GeneralResponse<List<MostSoldBooksDto>>
                {
                    IsSuccess = true,
                    Message = "Most Sold Books Listed Successfully.",
                    Data = Data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<MostSoldBooksDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong.",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<TopCategoryDto>> TopCategory()
        {
            try
            {
                var CategoryId = await _context.Books
                    .OrderByDescending(s => s.NumOfSoldCopies)
                    .Take(5)
                    .SelectMany(bc => bc.bookCategories.Select(i => i.categoryId))
                    .GroupBy(g => g)
                    .Select(n => new
                    {
                        CategoryId = n.Key,
                        Count = n.Count()
                    })
                    .OrderByDescending(o => o.Count)
                    .Select(h => h.CategoryId)
                    .FirstOrDefaultAsync();

                var categoryName = await _context.Categories.FindAsync(CategoryId);


                return new GeneralResponse<TopCategoryDto>
                {
                    IsSuccess = true,
                    Message = "Top Category is posted Successfully.",
                    Data = _mapper.Map<TopCategoryDto>(categoryName)
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<TopCategoryDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong.",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<TopCategoryDto>> TopCategoryForEachUser(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if(user == null)
                {
                    return new GeneralResponse<TopCategoryDto>
                    {
                        IsSuccess = false,
                        Message = "No User Found with this Id .",
                    };
                }
                var userBooks = await _context.bookUsers.Where(i => i.userId == userId).ToListAsync();
                if (userBooks == null)
                {
                    return new GeneralResponse<TopCategoryDto>
                    {
                        IsSuccess = false,
                        Message = "The User doesnot have a favourite category yet.",
                    };
                }
                var booksId = userBooks.Select(s => s.bookId).GroupBy(bookid => bookid).Select(ss => ss.Key).ToList();
                var CategoryId = await _context.bookCategories
                    .Where(w => booksId.Contains(w.bookId))
                    .Select(c => c.categoryId)
                    .GroupBy(g => g)
                    .Select(p => new
                    {
                        categoryid = p.Key,
                        count = p.Count()
                    })
                    .OrderByDescending(o => o.count)
                    .Select(v => v.categoryid)
                    .FirstOrDefaultAsync();
                var CategoryName = await _context.Categories.FindAsync(CategoryId);
                return new GeneralResponse<TopCategoryDto>
                {
                    IsSuccess = true,
                    Message = "Top Category is posted Successfully.",
                    Data = _mapper.Map<TopCategoryDto>(CategoryName)
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<TopCategoryDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong.",
                    Error = ex
                };
            }
        }


        public async Task<GeneralResponse<List<TopRatedDto>>> GetBooksByCategory(int categoryId)
        {
            try
            {
                var category = await _context.Categories.FindAsync(categoryId);
                if (category == null)
                {
                    return new GeneralResponse<List<TopRatedDto>>
                    {
                        IsSuccess = false,
                        Message = "No Category Found with this Id .",
                    };
                }
                var booksCategory= await _context.bookCategories.Where(i => i.categoryId == categoryId).Select(b => b.bookId).ToListAsync();
                var books = await _context.Books.Where(i => booksCategory.Contains(i.Id)).ToListAsync();
                var Data = _mapper.Map<List<TopRatedDto>>(books);
                foreach (var book in books)
                {
                    var names = await _context.bookCategories.Where(i => i.bookId == book.Id).Select(s => s.Category.Name).ToListAsync();
                    var data = Data.FirstOrDefault(i => i.id == book.Id);
                    data.Categories = names;
                }
                return new GeneralResponse<List<TopRatedDto>>
                {
                    IsSuccess = true,
                    Message = "Books  is listed Successfully.",
                    Data = Data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<TopRatedDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong.",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<UserActivityDto>> UserActivity(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return new GeneralResponse<UserActivityDto>
                    {
                        IsSuccess = false,
                        Message = "No User Found with this Id .",
                    };
                }
                var v = await _context.bookUsers.Where(i => i.userId == id).FirstOrDefaultAsync();
                var check = await _context.bookUsers.Where(i => i.userId == id).Select(s => s.Date).OrderByDescending(d => d.Date).FirstOrDefaultAsync();
                if(v == null)
                {
                    return new GeneralResponse<UserActivityDto>
                    {
                        IsSuccess = false,
                        Message = "No Buying Processes is done yet .",
                    };
                }
                var fromMonth = DateTime.Now.AddDays(-30);
                var isActive = (fromMonth > check) ? false : true;
                var data = new UserActivityDto
                {
                    IsActive = isActive,
                    LastBuyingProcess = check
                };
                return new GeneralResponse<UserActivityDto>
                {
                    IsSuccess = true,
                    Message = "this is the user status.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<UserActivityDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong.",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<List<TotalSalesDto>>> TotalSales(DateTime startDate, DateTime endDate)
        {
            try
            {
                var bu = await _context.bookUsers.Where(d => d.Date >= startDate && d.Date < endDate).ToListAsync();
                var data = bu.GroupBy(g => g.bookId).Select(s => new TotalSalesDto
                {
                    bookId = s.Key,
                    Copies = s.Sum(u => u.NumOfBoughtCopies),
                    price = s.Sum(m => m.price)
                })
                .ToList();
                var total = data.Select(e => e.price).Sum();

                
                return new GeneralResponse<List<TotalSalesDto>>
                {
                    IsSuccess = true,
                    Message = $"The Total Sales is {total}$ .",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<TotalSalesDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong.",
                    Error = ex
                };
            }
        }

        
    }
}
