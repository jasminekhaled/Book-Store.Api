using Shopping.Models;

namespace Shopping.Services
{
    public interface IUserServices
    {
        Task<User> SignUp(User user);
        Task<User> ResetPassword(User user);
        Task<User> ForgetPassword(User user);
        Task<bool> CheckTheUserName(string userName);
        Task<bool> CheckTheEmail(string email);
        Task<User> GetByPasswordAndEmail(string email, string password);
        Task<User> GetByUserNameAndEmail(string email, string userName);
        

    }
}
