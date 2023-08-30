using Shopping.Dtos.CartsDtos.ResponseDtos;
using Shopping.Dtos;
using Shopping.Dtos.StatisticsDtos.ResponseDtos;

namespace Shopping.Interfaces
{
    public interface IStatisticsServices
    {
        Task<GeneralResponse<List<TopRatedDto>>> TopRatedBooks();
        Task<GeneralResponse<List<MostSoldBooksDto>>> MostSoldBooksInGeneral();
        Task<GeneralResponse<List<MostSoldBooksDto>>> MostSoldBooksInPeriod(DateTime startDate, DateTime endDate);
        Task<GeneralResponse<TopCategoryDto>> TopCategory();
        Task<GeneralResponse<TopCategoryDto>> TopCategoryForEachUser(int userId);
        Task<GeneralResponse<List<TopRatedDto>>> GetBooksByCategory(int categoryId);
        Task<GeneralResponse<UserActivityDto>> UserActivity(int id);
        Task<GeneralResponse<List<TotalSalesDto>>> TotalSales(DateTime startDate, DateTime endDate);
        
    }
} 
