using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using System.Security.Cryptography;
using System;
using System.Text;
using Shopping.Helpers;
using Shopping.Dtos.RequestDtos;
using Shopping.Interfaces;

namespace Shopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;

        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;
        }


        
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpDto dto)
        {
            var result = await _authServices.SignUp(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }



        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn(LogInDto dto)
        {
            var result = await _authServices.LogIn(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }



        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            var result = await _authServices.ResetPassword(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }



        [HttpPut("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto dto)
        {
            var result = await _authServices.ForgetPassword(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

    }
}
