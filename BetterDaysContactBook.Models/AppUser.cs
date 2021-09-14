using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace BetterDaysContactBook.Models
{
    public class AppUser : IdentityUser
    {
        [Key]
        public override string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public override string Email { get; set; }
        public override string PhoneNumber { get; set; }
        public string FacebookAddress { get; set; }
        public string PhotoUrl { get; set; }
        public string Address { get; set; }
        
    }
}
