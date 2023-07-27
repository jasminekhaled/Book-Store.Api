using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shopping.Context;
using Shopping.Dtos.RequestDtos;
using Shopping.Helpers;
using Shopping.Models;

namespace Shopping.Services
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserServices(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

            var user = _mapper.Map<User>(dto);
  
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return (user);
        }
        public async Task LogIn(LogInDto dto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == dto.Email && x.Password == HashingService.HashPassword(dto.Password));

            if (user == null)
            {
                throw new Exception( $"Email or Password is wrong");
            }
            return ;
        }

        public async Task ForgetPassword(ForgetPasswordDto dto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == dto.Email && x.UserName == dto.UserName);
            if (!await _context.Users.AnyAsync(x => x.UserName == dto.UserName))
            {
                throw new Exception($"Wrong UserName!");
            }
            if (user == null)
            {
                throw new Exception($"No User Exist with this Email!");
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return ;
        }

        public async Task ResetPassword(ResetPasswordDto dto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == dto.Email && x.Password == HashingService.HashPassword(dto.Password));

            if (!await _context.Users.AnyAsync(x => x.Email == dto.Email))
            {
                throw new Exception($"No User Exist with this Email!");
            }

            if (user == null)
            {
                throw new Exception($"Wrong Password");
            }

            user.Password = HashingService.HashPassword(dto.NewPassword);
         
            var configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json")
           .Build();

            string DefaultPassword = configuration.GetValue<string>("DefaultPassword");

            user.Password = HashingService.HashPassword(DefaultPassword);

            await _context.SaveChangesAsync();
            return ;
        }

       
        
    }
}
