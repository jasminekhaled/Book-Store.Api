using Shopping.Dtos.RequestDtos;
using Shopping.Dtos.ResponseDtos;
using Shopping.Models;

namespace Shopping.Services
{
    public interface IUserServices
    {
        Task<GeneralResponse<UserDto>> SignUp(SignUpDto dto);
        Task<GeneralResponse<UserDto>> LogIn(LogInDto dto);
        Task<GeneralResponse<UserDto>> ResetPassword(ResetPasswordDto dto);
        Task<GeneralResponse<UserDto>> ForgetPassword(ForgetPasswordDto dto);
       

    }
}
