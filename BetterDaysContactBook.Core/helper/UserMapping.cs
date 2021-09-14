using BetterDaysContactBook.Models;
using BetterDaysContactBook.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterDaysContactBook.Core.helper
{
    public static class UserMapping
    {
        public static UserDTO GetUserResponse(AppUser user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                Name = $"{user.FirstName} {user.LastName}",
                PhoneNumber = user.PhoneNumber,
                Age = AgeCalculator.GetCurrentAge(user.DateOfBirth),
                FacebookAddress = user.FacebookAddress,
                PhotoUrl = user.PhotoUrl,
                Address = user.Address
            };
        }

        public static List<UserDTO> GetUserSearchResponse(List<AppUser> user)
        {
            List<UserDTO> userDTOs = new List<UserDTO>();
            foreach (var item in user)
            {
                var d = new UserDTO
                {
                    Id = item.Id,
                    Email = item.Email,
                    Name = $"{item.FirstName} {item.LastName}",
                    PhoneNumber = item.PhoneNumber,
                    Age = AgeCalculator.GetCurrentAge(item.DateOfBirth),
                    FacebookAddress = item.FacebookAddress,
                    PhotoUrl = item.PhotoUrl,
                    Address = item.Address
                };
                userDTOs.Add(d);
            }
            return userDTOs;
        }


        public static AppUser GetRegisterUser(RegisterDTO registerUser)
        {
            return new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = registerUser.Email,
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                PhoneNumber = registerUser.PhoneNumber,
                PhotoUrl = registerUser.PhotoUrl,
                UserName = string.IsNullOrWhiteSpace(registerUser.UserName) ? registerUser.Email : registerUser.UserName,
                Address = registerUser.Address,
                Gender = registerUser.Gender,
                FacebookAddress = registerUser.FacebookAddress,
                DateOfBirth = registerUser.DateOfBirth
            };


        }
    }
}
