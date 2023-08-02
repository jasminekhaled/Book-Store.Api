using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopping.Dtos.BookDtos.RequestDtos;
using Shopping.Interfaces;

namespace Shopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookServices _bookServices;

        public BooksController(IBookServices bookServices)
        {
            _bookServices = bookServices;
        }


        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _bookServices.GetAllCategories();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory(AddCategoryDto dto)
        {
            var result = await _bookServices.AddCategory(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }



        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _bookServices.DeleteCategory(id);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }



        [HttpGet("GetAllBooks")]
        public async Task<IActionResult> GetAllBooks()
        {
            var result = await _bookServices.GetAllBooks();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }



        [HttpPost("AddBook")]
        public async Task<IActionResult> AddBook([FromForm] BookRequestDto dto)
        {
            var result = await _bookServices.AddBook(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }



        [HttpDelete("DeleteBook")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var result = await _bookServices.DeleteBook(id);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }



        [HttpGet("BookDetails")]
        public async Task<IActionResult> BookDetails(int id)
        {
            var result = await _bookServices.BookDetails(id);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

    }
}
