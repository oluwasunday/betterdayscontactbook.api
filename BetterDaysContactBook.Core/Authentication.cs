using BetterDaysContactBook.Common;
using BetterDaysContactBook.Core.helper;
using BetterDaysContactBook.Models;
using BetterDaysContactBook.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterDaysContactBook.Core
{
    public class Authentication : IAuthentication
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenGenerator _tokenGenerator;

        public Authentication(UserManager<AppUser> userNanager, ITokenGenerator tokenGenerator)
        {
            _userManager = userNanager;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<UserDTO> Login(LoginDTO loginRequest)
        {
            AppUser user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user != null)
            {
                LoggedUser.LoggedInUserId = user.Id;
                if (await _userManager.CheckPasswordAsync(user, loginRequest.Password))
                {
                    var response = UserMapping.GetUserResponse(user);
                    response.Token = await _tokenGenerator.GenerateToken(user);
                    return response;
                }
                throw new AccessViolationException("Invalid login details");
            }
            throw new AccessViolationException("Invalid login details");
        }


        public async Task<UserDTO> Register(RegisterDTO registerRequest)
        {
            AppUser user = UserMapping.GetRegisterUser(registerRequest);

            IdentityResult result = await _userManager.CreateAsync(user, registerRequest.Password);
            await _userManager.AddToRoleAsync(user, "Regular");

            if (result.Succeeded)
            {
                return UserMapping.GetUserResponse(user);
            }

            string errors = string.Empty;
            foreach (var error in result.Errors)
            {
                errors += error.Description + Environment.NewLine;
            }

            throw new MissingFieldException(errors);
        }
    }
}
