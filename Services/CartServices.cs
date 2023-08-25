using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shopping.Context;
using Shopping.Dtos;
using Shopping.Dtos.CartsDtos.RequestDtos;
using Shopping.Dtos.CartsDtos.ResponseDtos;
using Shopping.Interfaces;
using Shopping.Models.CartModule;
using System.Linq;

namespace Shopping.Services
{
    public class CartServices : ICartServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CartServices(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GeneralResponse<List<CartBooksDto>>> ListOfBooksInCart(int id)
        {
            try
            {
                if(!await _context.Carts.AnyAsync(i => i.Id == id))
                {
                    return new GeneralResponse<List<CartBooksDto>>
                    {
                        IsSuccess = false,
                        Message = "No cart found with this Id!",
                    };
                }
                var books = await _context.CartBooks.Where(c => c.cartId == id).ToListAsync();

                return new GeneralResponse<List<CartBooksDto>>
                {
                    IsSuccess = true,
                    Message = "Books Listed Successfully.",
                    Data = _mapper.Map<List<CartBooksDto>>(books)
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<CartBooksDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong.",
                    Error = ex
                };
            }

        }
        public async Task<GeneralResponse<CartDto>> AddToCart(int userId,int cartId, int bookId, AddDto dto)
        {
            try 
            {
                var user = await _context.Users.FindAsync(userId);
                if(user == null)
                {
                    return new GeneralResponse<CartDto>
                    {
                        IsSuccess = false,
                        Message = "No user found with this Id."
                    };
                }

                var cart = await _context.Carts.SingleOrDefaultAsync(u => u.UserId == userId);
                if (cart.Id != cartId)
                {
                    return new GeneralResponse<CartDto>
                    {
                        IsSuccess = false,
                        Message = "Wrong Cart Id."
                    };
                }

                var book = await _context.Books.FindAsync(bookId);
                if (book == null)
                {
                    return new GeneralResponse<CartDto>
                    {
                        IsSuccess = false,
                        Message = "No book found with this Id."
                    };
                }
                if(await _context.CartBooks.Where(n => n.cartId == cartId).AnyAsync(i => i.bookId == bookId))
                {
                    return new GeneralResponse<CartDto>
                    {
                        IsSuccess = false,
                        Message = "this book is already added in your cart."
                    };
                }    
                if (dto.wantedCopies > book.NumOfCopies || dto.wantedCopies <= 0)
                {
                    return new GeneralResponse<CartDto>
                    {
                        IsSuccess = false,
                        Message = "the number of wanted copies isnot available."
                    };
                }
                var BookPrice = book.Price * dto.wantedCopies;
                cart.TotalPrice = cart.TotalPrice + BookPrice;
                

                var cartBook = new CartBooks
                {
                    bookId = book.Id,
                    cartId = cart.Id, 
                    WantedCopies = dto.wantedCopies,
                    Price = BookPrice
                };

                await _context.CartBooks.AddRangeAsync(cartBook);
                await _context.SaveChangesAsync();

                var data = _mapper.Map<CartDto>(book);
                data.WantedCopies = dto.wantedCopies;
                data.Price = BookPrice;

                return new GeneralResponse<CartDto>
                {
                    IsSuccess = true,
                    Message = "The Book is added to the cart successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<CartDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong.",
                    Error = ex
                };
            }
        }


        public async Task<GeneralResponse<CartBooksDto>> DeleteFromCart(int cartId, int bookId)
        {
            try 
            {
                var cart = await _context.Carts.FindAsync(cartId);
                if (cart == null)
                {
                    return new GeneralResponse<CartBooksDto>
                    {
                        IsSuccess = false,
                        Message = "No Cart found with this Id."
                    };
                }

               var check = await _context.CartBooks.Where(n => n.cartId == cartId).SingleOrDefaultAsync(i => i.bookId == bookId);
                if (check == null)
                {
                    return new GeneralResponse<CartBooksDto>
                    {
                        IsSuccess = false,
                        Message = "No book found in your cart with this Id."
                    };
                }

                _context.CartBooks.Remove(check);
                _context.SaveChanges();

                return new GeneralResponse<CartBooksDto>
                {
                    IsSuccess = true,
                    Message = "The Book is deleted from the cart successfully.",
                    Data = _mapper.Map<CartBooksDto>(check)
            };

            }
            catch (Exception ex)
            {
                return new GeneralResponse<CartBooksDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong.",
                    Error = ex
                };
            }


        }
        
        public Task<GeneralResponse<List<CartDto>>> Buying(int userId, int cartId)
        {
            throw new NotImplementedException();
        }

        
    }
}
