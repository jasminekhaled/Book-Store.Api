using Shopping.Dtos;
using Shopping.Dtos.BookDtos.RequestDtos;
using Shopping.Dtos.BookDtos.ResponseDtos;
using Shopping.Models;

namespace Shopping.Interfaces
{
    public interface IBookServices
    {
        Task<GeneralResponse<List<CategoryDto>>> GetAllCategories();
        Task<GeneralResponse<List<BookDto>>> GetAllBooks();
        Task<GeneralResponse<CategoryDto>> AddCategory(AddCategoryDto dto);
        Task<GeneralResponse<CategoryDto>> DeleteCategory(int id);
        Task<GeneralResponse<BookDto>> AddBook(BookRequestDto dto);
        Task<GeneralResponse<BookDto>> DeleteBook(int id);
        Task<GeneralResponse<BookDto>> BookDetails(int id);
        Task<GeneralResponse<BookDto>> EditBook(int id, EditRequestDto dto);
        Task<GeneralResponse<BookDto>> BuyABook(int id);
        Task<GeneralResponse<List<BookDto>>> GetBooksByUser(int id);

    }
}
