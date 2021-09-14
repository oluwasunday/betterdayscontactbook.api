using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterDaysContactBook.Models.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string PhotoUrl { get; set; } = "default.jpg";
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string FacebookAddress { get; set; }
        public string Address { get; set; }
    }
}
