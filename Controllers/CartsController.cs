using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopping.Dtos.CartsDtos.RequestDtos;
using Shopping.Interfaces;
using Shopping.Services;

namespace Shopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartServices _cartServices;

        public CartsController(ICartServices cartServices)
        {
            _cartServices = cartServices;
        }


        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(int userId, int cartId, int bookId, AddDto dto)
        {
            var result = await _cartServices.AddToCart(userId, cartId, bookId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("ListOfBooksInCart")]
        public async Task<IActionResult> ListOfBooksInCart(int id)
        {
            var result = await _cartServices.ListOfBooksInCart(id);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
