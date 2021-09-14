using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterDaysContactBook.Models.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Name { get; set; } 
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; } 
        public string Email { get; set; }
        public string PhotoUrl { get; set; }
        public string FacebookAddress { get; set; }
        public string Token { get; set; } 
    }
}
