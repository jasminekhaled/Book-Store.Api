﻿using System.ComponentModel.DataAnnotations;

namespace Shopping.Dtos
{
    public class ResetPasswordDtocs
    {
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(length: 10)]
        public string Password { get; set; }

        [MinLength(length: 10)]
        public string NewPassword { get; set; }
    }
}
