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
            var result = await _userServices.SignUp(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }



        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn(LogInDto dto)
        {
            var result = await _userServices.LogIn(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }



        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            var result = await _userServices.ResetPassword(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }



        [HttpPut("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto dto)
        {
            var result = await _userServices.ForgetPassword(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

    }
}
