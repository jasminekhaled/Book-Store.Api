using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopping.Dtos.CartsDtos.RequestDtos;
using Shopping.Interfaces;
using Shopping.Services;

namespace Shopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsServices _statisticsServices;
        public StatisticsController(IStatisticsServices statisticsServices)
        {
            _statisticsServices = statisticsServices;
        }

        [HttpGet("TopRatedBooks")]
        public async Task<IActionResult> TopRatedBooks()
        {
            var result = await _statisticsServices.TopRatedBooks();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("MostSoldBooksInGeneral")]
        public async Task<IActionResult> MostSoldBooksInGeneral()
        {
            var result = await _statisticsServices.MostSoldBooksInGeneral();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


        [HttpGet("MostSoldBooksInPeriod")]
        public async Task<IActionResult> MostSoldBooksInPeriod(DateTime startDate, DateTime endDate)
        {
            var result = await _statisticsServices.MostSoldBooksInPeriod(startDate, endDate);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


        [HttpGet("TopCategory")]
        public async Task<IActionResult> TopCategory()
        {
            var result = await _statisticsServices.TopCategory();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("TopCategoryForEachUser")]
        public async Task<IActionResult> TopCategoryForEachUser(int userId)
        {
            var result = await _statisticsServices.TopCategoryForEachUser(userId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


        [HttpGet("UserActivity")]
        public async Task<IActionResult> UserActivity(int id)
        {
            var result = await _statisticsServices.UserActivity(id);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


        [HttpGet("TotalSales")]
        public async Task<IActionResult> TotalSales(DateTime startDate, DateTime endDate)
        {
            var result = await _statisticsServices.TotalSales(startDate, endDate);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


        [HttpGet("GetBooksByCategory")]
        public async Task<IActionResult> GetBooksByCategory(int categoryId)
        {
            var result = await _statisticsServices.GetBooksByCategory(categoryId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
