using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shopping.Context;
using Shopping.Dtos;
using Shopping.Dtos.CartsDtos.RequestDtos;
using Shopping.Dtos.CartsDtos.ResponseDtos;
using Shopping.Interfaces;
using Shopping.Migrations;
using Shopping.Models.BookModule;
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
                
                cart.TotalPrice = cart.TotalPrice - check.Price;
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

        public async Task<GeneralResponse<CartBooksDto>> EditOnNumOfCopies(int cartId, int bookId, AddDto dto)
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

                var book = await _context.Books.FindAsync(bookId);
                if(dto.wantedCopies > book.NumOfCopies || dto.wantedCopies <= 0)
                {
                    return new GeneralResponse<CartBooksDto>
                    {
                        IsSuccess = false,
                        Message = "Num of wanted copies isnot available."
                    };
                }

                check.WantedCopies = dto.wantedCopies;
                cart.TotalPrice -= check.Price;

                check.Price = dto.wantedCopies * book.Price;
                cart.TotalPrice += check.Price;

                _context.Carts.Update(cart);
                _context.CartBooks.Update(check);
                _context.SaveChanges();

                return new GeneralResponse<CartBooksDto>
                {
                    IsSuccess = true,
                    Message = "The number of wanted copies is changed successfully.",
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


        public async Task<GeneralResponse<List<CartBooksDto>>> Buying(int userId, int cartId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return new GeneralResponse<List<CartBooksDto>>
                    {
                        IsSuccess = false,
                        Message = "No user found with this Id."
                    };
                }

                var cart = await _context.Carts.SingleOrDefaultAsync(u => u.UserId == userId);
                if (cart.Id != cartId)
                {
                    return new GeneralResponse<List<CartBooksDto>>
                    {
                        IsSuccess = false,
                        Message = "Wrong Cart Id."
                    };
                }

                

                var cartbooks = await _context.CartBooks.Where(c => c.cartId == cartId).ToListAsync();
                if (cartbooks == null)
                {
                    return new GeneralResponse<List<CartBooksDto>>
                    {
                        IsSuccess = false,
                        Message = "The Cart is Empty."
                    };
                }
                foreach (var cartbook in cartbooks)
                {
                    var book = await _context.Books.FindAsync(cartbook.bookId);
                    if (book.NumOfCopies == 0)
                    {
                        var TheCart = await _context.Carts.FindAsync(cartbook.cartId);
                        TheCart.TotalPrice = TheCart.TotalPrice - cartbook.Price;
                        _context.CartBooks.Remove(cartbook);
                        _context.SaveChanges();
                        return new GeneralResponse<List<CartBooksDto>>
                        {
                            IsSuccess = false,
                            Message = $"The book with this Id => {cartbook.bookId} is sold out."
                        };

                    }
                    if (book.NumOfCopies < cartbook.WantedCopies)
                    {
                        return new GeneralResponse<List<CartBooksDto>>
                        {
                            IsSuccess = false,
                            Message = $"The number of copies for the book with this Id => {cartbook.bookId} isnot available."
                        };

                    }
                }

                var bookUsers = cartbooks.Select(s => new BookUsers
                {
                    bookId = s.bookId,
                    userId = userId,
                    Date = DateTime.Now,
                    NumOfBoughtCopies = s.WantedCopies
                }).ToList();
                await _context.bookUsers.AddRangeAsync(bookUsers);

                foreach (var cartbook in cartbooks)
                {
                    var book = await _context.Books.FindAsync(cartbook.bookId);
                    book.NumOfCopies -=  cartbook.WantedCopies;
                    book.NumOfSoldCopies += cartbook.WantedCopies;
                    _context.Books.Update(book);
                    _context.SaveChanges();
                }
                

                cart.TotalPrice = 0;
                _context.Carts.Update(cart);
                var data = _mapper.Map<List<CartBooksDto>>(cartbooks);
                _context.CartBooks.RemoveRange(cartbooks);
                _context.SaveChanges();

                return new GeneralResponse<List<CartBooksDto>>
                {
                    IsSuccess = true,
                    Message = "The Process had been done successfully.",
                    Data = data
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

        
    }
}
