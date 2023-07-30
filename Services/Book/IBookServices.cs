using Shopping.Dtos;
using Shopping.Dtos.BookDtos.RequestDtos;
using Shopping.Dtos.BookDtos.ResponseDtos;
using Shopping.Models;

namespace Shopping.Services.Book
{
    public interface IBookServices
    {
        Task<GeneralResponse<List<CategoryDto>>> GetAllCategories();
        Task<GeneralResponse<List<BookDto>>> GetAllBooks();
        Task<GeneralResponse<CategoryDto>> AddCategory(AddCategoryDto dto);
        Task<GeneralResponse<CategoryDto>> DeleteCategory(int id);
        Task<GeneralResponse<BookDto>> AddBook(BookRequestDto dto);
        Task<GeneralResponse<BookDto>> DeleteBook(int id);
        Task<GeneralResponse<BookDto>> BookDetails(BookRequestDto dto);
        Task<GeneralResponse<BookDto>> EditBook(BookRequestDto dto);
        Task<GeneralResponse<BookDto>> BuyABook(int id);
        Task<GeneralResponse<List<BookDto>>> GetBooksByUser(int id);

    }
}
