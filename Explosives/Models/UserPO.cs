using System;
using System.ComponentModel.DataAnnotations;

namespace Explosives.Models
{
    public class UserPO
    {
        public Int64 UserID { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Please enter a Username between 5-20 characters")]
        public string Username { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Please enter a Password between 5-20 characters")]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(150)]
        [EmailAddress]
        public string Email { get; set; }

        public int RoleID { get; set; }
    }
}