using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shopping.Context;
using Shopping.Dtos;
using Shopping.Dtos.RequestDtos;
using Shopping.Dtos.ResponseDtos;
using Shopping.Helpers;
using Shopping.Interfaces;
using Shopping.Models.AuthModule;

namespace Shopping.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthServices(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<GeneralResponse<UserDto>> SignUp(SignUpDto dto)
        {
            try
            {
                if (await _context.Users.AnyAsync(x => x.UserName == dto.UserName))
                {
                    return new GeneralResponse<UserDto>
                    {
                        IsSuccess = false,
                        Message = "This username is already taken."
                    };
                }
                if (await _context.Users.AnyAsync(x => x.Email == dto.Email))
                {
                    return new GeneralResponse<UserDto>
                    {
                        IsSuccess = false,
                        Message = "User is already Exist."
                    };
                }

                var user = _mapper.Map<User>(dto);
                user.Password = HashingService.HashPassword(dto.Password);


                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return new GeneralResponse<UserDto>
                {
                    IsSuccess = true,
                    Message = "Signed Up Successfully",
                    Data = _mapper.Map<UserDto>(dto)
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<UserDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }

        }

        public async Task<GeneralResponse<UserDto>> LogIn(LogInDto dto)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == dto.Email && x.Password == HashingService.HashPassword(dto.Password));

                if (user == null)
                {
                    return new GeneralResponse<UserDto>
                    {
                        IsSuccess = false,
                        Message = "Email or Password is wrong."
                    };
                }
                return new GeneralResponse<UserDto>
                {
                    IsSuccess = true,
                    Message = "Logged In Successfully",
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<UserDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<UserDto>> ResetPassword(ResetPasswordDto dto)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == dto.Email && x.Password == HashingService.HashPassword(dto.Password));

                if (!await _context.Users.AnyAsync(x => x.Email == dto.Email))
                {
                    return new GeneralResponse<UserDto>
                    {
                        IsSuccess = false,
                        Message = "No User Exist with this Email!"
                    };
                }

                if (user == null)
                {
                    return new GeneralResponse<UserDto>
                    {
                        IsSuccess = false,
                        Message = "Wrong Password."
                    };
                }

                user.Password = HashingService.HashPassword(dto.NewPassword);

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return new GeneralResponse<UserDto>
                {
                    IsSuccess = true,
                    Message = "Password is resetted successfully",
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<UserDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }

        }

        public async Task<GeneralResponse<UserDto>> ForgetPassword(ForgetPasswordDto dto)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == dto.Email && x.UserName == dto.UserName);
                if (!await _context.Users.AnyAsync(x => x.UserName == dto.UserName))
                {
                    return new GeneralResponse<UserDto>
                    {
                        IsSuccess = false,
                        Message = "Wrong UserName!"
                    };
                }
                if (user == null)
                {
                    return new GeneralResponse<UserDto>
                    {
                        IsSuccess = false,
                        Message = "No User Exist with this Email!"
                    };
                }

                string DefaultPassword = _configuration.GetValue<string>("DefaultPassword");

                user.Password = HashingService.HashPassword(DefaultPassword);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return new GeneralResponse<UserDto>
                {
                    IsSuccess = true,
                    Message = "User has a temperory Default Password",
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<UserDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }

        }





    }
}
