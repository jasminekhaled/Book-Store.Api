using Microsoft.EntityFrameworkCore;
using Shopping.Dtos;
using Shopping.Helpers;
using Shopping.Models;

namespace Shopping.Services
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationDbContext _context;

        public UserServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> SignUp(SignUpDto dto)
        {
            if (await _context.Users.AnyAsync(x => x.UserName == dto.UserName))
            {
                throw new Exception($"This username is already taken.");
            }
            if (await _context.Users.AnyAsync(x => x.Email == dto.Email))
            {
                throw new Exception($"User is already Exist.");
            }

            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                Password = HashingService.HashPassword(dto.Password)
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return (user);
        }

        public async Task<User> ForgetPassword(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return (user);
        }

        public async Task<User> ResetPassword(User user)
        {
            var configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json")
           .Build();

            string DefaultPassword = configuration.GetValue<string>("DefaultPassword");

            user.Password = HashingService.HashPassword(DefaultPassword);

            await _context.SaveChangesAsync();
            return (user);
        }

        public async Task<bool> CheckTheUserName(string userName)
        {
            return await _context.Users.AnyAsync(x => x.UserName == userName);
        }
        public async Task<bool> CheckTheEmail(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<User> GetByPasswordAndEmail(string email, string password)
        {
           return await _context.Users.SingleOrDefaultAsync(x => x.Email == email && x.Password == HashingService.HashPassword(password));
        }

        public async Task<User> GetByUserNameAndEmail(string email, string userName)
        {
           return await _context.Users.SingleOrDefaultAsync(x => x.Email == email && x.UserName == userName);
        }

       

        
    }
}
