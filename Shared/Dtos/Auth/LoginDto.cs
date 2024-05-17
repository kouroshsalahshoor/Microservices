﻿using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Auth
{
    public class LoginDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
