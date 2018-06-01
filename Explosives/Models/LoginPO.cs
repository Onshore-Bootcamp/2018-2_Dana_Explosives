using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Explosives.Models
{
    public class LoginPO
    {
        [Required]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Please enter a Username between 5-20 characters")]
        public string Username { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Please enter a Username between 5-20 characters")]
        public string Password { get; set; }
    }
}