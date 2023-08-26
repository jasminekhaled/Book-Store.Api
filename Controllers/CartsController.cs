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

        [HttpDelete("DeleteFromCart")]
        public async Task<IActionResult> DeleteFromCart(int cartId, int bookId)
        {
            var result = await _cartServices.DeleteFromCart(cartId, bookId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("EditOnNumOfCopies")]
        public async Task<IActionResult> EditOnNumOfCopies(int cartId, int bookId, AddDto dto)
        {
            var result = await _cartServices.EditOnNumOfCopies(cartId, bookId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("Buying")]
        public async Task<IActionResult> Buying(int userId, int cartId)
        {
            var result = await _cartServices.Buying(userId, cartId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
