using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertAPI.DTOs.Requests
{
    public class LoginRequestDto
    {
        [Required]
        [MaxLength(100)]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
