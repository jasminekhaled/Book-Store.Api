using Shopping.Dtos;
using Shopping.Dtos.RequestDtos;
using Shopping.Dtos.ResponseDtos;
using Shopping.Models;

namespace Shopping.Interfaces
{
    public interface IAuthServices
    {
        Task<GeneralResponse<UserDto>> SignUp(SignUpDto dto);
        Task<GeneralResponse<UserDto>> LogIn(LogInDto dto);
        Task<GeneralResponse<UserDto>> ResetPassword(ResetPasswordDto dto);
        Task<GeneralResponse<UserDto>> ForgetPassword(ForgetPasswordDto dto);


    }
}
