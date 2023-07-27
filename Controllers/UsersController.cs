using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using System.Security.Cryptography;
using System;
using System.Text;
using Shopping.Helpers;
using Shopping.Services;
using Shopping.Dtos.RequestDtos;

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
            await _userServices.LogIn(dto);
            return Ok("Logged in successfully");
        }



        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            await _userServices.ResetPassword(dto);
            return Ok("The Password Resetted Successfully");
        }



        [HttpPut("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto dto)
        {
            await _userServices.ForgetPassword(dto);
            return Ok();
        }

    }
}
