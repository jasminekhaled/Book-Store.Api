using Shopping.Dtos.RequestDtos;
using Shopping.Models;

namespace Shopping.Services
{
    public interface IUserServices
    {
        Task<User> SignUp(SignUpDto dto);
        Task LogIn(LogInDto dto);
        Task ResetPassword(ResetPasswordDto dto);
        Task ForgetPassword(ForgetPasswordDto dto);
       

    }
}
