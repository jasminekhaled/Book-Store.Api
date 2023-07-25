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
using Shopping.Services;

namespace Shopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public UsersController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpDto dto)
        {
            await _userServices.SignUp(dto);
            return Ok(value: $"Signed Up successfully");
        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn(LogInDto dto)
        {
            var user = await _userServices.GetByPasswordAndEmail(dto.Email, dto.Password);

            if (user == null)
            {
                return BadRequest(error: $"Email or Password is wrong");
            }

            return Ok("Logged in successfully");
        }

        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            var user = await _userServices.GetByPasswordAndEmail(dto.Email, dto.Password);

            if (!await _userServices.CheckTheEmail(dto.Email))
            {
                return BadRequest(error: $"No User Exist with this Email!");
            }

            if (user == null)
            {
                return BadRequest(error: $"Wrong Password");
            }

             user.Password = HashingService.HashPassword(dto.NewPassword);
             await _userServices.ResetPassword(user);

            return Ok("The Password Resetted Successfully");
        }

        [HttpPut("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto dto)
        {
            var user = await _userServices.GetByUserNameAndEmail(dto.Email, dto.UserName);
            if(!await _userServices.CheckTheUserName(dto.UserName))
            {
                return BadRequest(error: $"Wrong UserName!");
            }
            if(user == null)
            {
                return BadRequest(error: $"No User Exist with this Email!");
            }

            await _userServices.ForgetPassword(user);

            return Ok(user);
        }

    }
}
