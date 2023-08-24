using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shopping.Context;
using Shopping.Dtos;
using Shopping.Dtos.CartsDtos.RequestDtos;
using Shopping.Dtos.CartsDtos.ResponseDtos;
using Shopping.Interfaces;
using Shopping.Models.CartModule;

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

        public async Task<GeneralResponse<List<ListOfBooksDto>>> ListOfBooksInCart(int id)
        {
            try
            {
                if(!await _context.Carts.AnyAsync(i => i.Id == id))
                {
                    return new GeneralResponse<List<ListOfBooksDto>>
                    {
                        IsSuccess = false,
                        Message = "No cart found with this Id!",
                    };
                }
                var books = await _context.CartBooks.Where(c => c.cartId == id).ToListAsync();

                return new GeneralResponse<List<ListOfBooksDto>>
                {
                    IsSuccess = true,
                    Message = "Books Listed Successfully.",
                    Data = _mapper.Map<List<ListOfBooksDto>>(books)
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<ListOfBooksDto>>
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

                var cart = await _context.Carts.FindAsync(cartId);
                if (cart == null)
                {
                    return new GeneralResponse<CartDto>
                    {
                        IsSuccess = false,
                        Message = "No Cart found with this Id."
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

        public Task<GeneralResponse<List<CartDto>>> Buying(int userId, int cartId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<CartDto>> DeleteFromCart(int userId, int bookId)
        {
            throw new NotImplementedException();
        }

        
    }
}
