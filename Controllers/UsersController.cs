using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Dtos;
using Shopping.Models;
using System.Security.Cryptography;
using System;
using System.Text;
using Shopping.Helpers;

namespace Shopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpDto dto)
        {
            
            if (await _context.Users.AnyAsync(x => x.UserName == dto.UserName))
            {
                return BadRequest(error: $"This username is already taken.");
            }
            if (await _context.Users.AnyAsync(x => x.Email == dto.Email))
            {
                return BadRequest(error: $"User is already Exist");
            }

            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                Password = HashingService.HashPassword(dto.Password)
            };

           
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
 
            return Ok(user);
        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn(LogInDto dto)
        {
            var hash = HashingService.HashPassword(dto.Password);
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Password == hash && x.Email == dto.Email);
       

            if (user == null)
            {
                return BadRequest(error: $"Email or Password is wrong");
            }

            return Ok("Logged in successfully");
        }

        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDtocs dto)
        {

            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == dto.Email && x.Password == HashingService.HashPassword(dto.Password));

            if (!await _context.Users.AnyAsync(x => x.Email == dto.Email))
            {
                return BadRequest(error: $"No User Exist with this Email!");
            }

            if (user == null)
            {
                return BadRequest(error: $"Wrong Password");
            }

             user.Password = HashingService.HashPassword(dto.NewPassword);
             _context.Users.Update(user);
             await _context.SaveChangesAsync();

            return Ok("The Password Resetted Successfully");


        }

        [HttpPut("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto dto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == dto.Email && x.UserName == dto.UserName);
            if(!await _context.Users.AnyAsync(x => x.UserName == dto.UserName))
            {
                return BadRequest(error: $"Wrong UserName!");
            }
            if(user == null)
            {
                return BadRequest(error: $"No User Exist with this Email!");
            }

            string DefaultPassword = how to get value from appsetting?;

            user.Password = HashingService.HashPassword(DefaultPassword);

            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
