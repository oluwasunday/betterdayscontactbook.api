using AutoMapper;
using BetterDaysContactBook.Common;
using BetterDaysContactBook.Core.helper;
using BetterDaysContactBook.Models;
using BetterDaysContactBook.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterDaysContactBook.Core
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IContactBookRepository _contactBookRepository;
        private readonly IMapper _mapper;

        public UserService(UserManager<AppUser> userManager, 
            IContactBookRepository contactBookRepository, IMapper mapper)
        {
            _userManager = userManager;
            _contactBookRepository = contactBookRepository;
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }


        public async Task<PagedList<UserDTO>> GetAllUsers(PagingParams paging)
        {
            var users = await _userManager.Users.ToListAsync();

            if (users != null)
            {
                var map = _mapper.Map<IEnumerable<UserDTO>>(users).ToList();
                return PagedList<UserDTO>.Create(map, paging.PageNumber, paging.PageSize);
            }
            throw new ArgumentException("User not found!");
        }


        public async Task<UserDTO> GetUserById(string id)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id);
            if (appUser != null)
                return _mapper.Map<UserDTO>(appUser);

            throw new KeyNotFoundException("User not found!");
        }


        public async Task<UserDTO> GetUserByEmail(string email)
        {
            AppUser appUser = await _userManager.FindByEmailAsync(email);
            if (appUser != null)
                return _mapper.Map<UserDTO>(appUser);

            throw new KeyNotFoundException("User not found!");
        }


        public async Task<bool> Update(string userId, UpdateUserDTO updateUser)
        {
            AppUser appUser = await _userManager.FindByIdAsync(userId);
            if (appUser != null)
            {
                appUser.FirstName = (string.IsNullOrWhiteSpace(updateUser.FirstName) || updateUser.FirstName == "string") ? 
                    appUser.FirstName : updateUser.FirstName;
                appUser.LastName = (string.IsNullOrWhiteSpace(updateUser.LastName) || updateUser.LastName == "string") ? 
                    appUser.LastName : updateUser.LastName;
                appUser.PhoneNumber = (string.IsNullOrWhiteSpace(updateUser.PhoneNumber) || updateUser.PhoneNumber == "string") ? 
                    appUser.PhoneNumber : updateUser.PhoneNumber;
                appUser.Address = (string.IsNullOrWhiteSpace(updateUser.Address) || updateUser.Address == "string") ? 
                    appUser.Address : updateUser.Address;

                var result = await _userManager.UpdateAsync(appUser);
                if (result.Succeeded)
                    return true;

                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += error.Description + Environment.NewLine;
                }

                throw new MissingMemberException(errors);
            }
            throw new ArgumentNullException("User not found!");
        }


        public async Task<bool> UpdatePhotoUrl(string newPhotoUrl)
        {
            var userId = LoggedUser.LoggedInUserId;
            
            AppUser appUser = await _userManager.FindByIdAsync(userId);
            if (appUser != null)
            {
                appUser.PhotoUrl = string.IsNullOrWhiteSpace(newPhotoUrl) ? "default.jpg" : newPhotoUrl;

                var result = await _userManager.UpdateAsync(appUser);
                if (result.Succeeded) 
                    return true;

                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += error.Description + Environment.NewLine;
                }

                if(!string.IsNullOrEmpty(errors))
                    throw new MissingMemberException(errors);
            }
            throw new ArgumentNullException("User not found!\nMake sure you login, and try again");
        }


        public async Task<bool> DeleteUser(string userId)
        {
            AppUser appUser = await _userManager.FindByIdAsync(userId);

            if (appUser != null)
            {
                var result = await _userManager.DeleteAsync(appUser);
                if (result.Succeeded)
                    return true;

                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += error.Description + Environment.NewLine;
                }

                throw new MissingMemberException(errors);
            }
            throw new ArgumentNullException("User not found!");
        }


        public async Task<UserDTO> AddNewUser(RegisterDTO registerRequest)
        {
            AppUser user = UserMapping.GetRegisterUser(registerRequest);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IdentityResult result = await _userManager.CreateAsync(user, registerRequest.Password);
            if (result.Succeeded)
                return _mapper.Map<UserDTO>(user);

            string errors = string.Empty;
            foreach (var error in result.Errors)
            {
                errors += error.Description + Environment.NewLine;
            }

            throw new MissingFieldException(errors);
        }


        public List<UserDTO> SearchUsersByTerm(string searchTerm)
        {
            var user = _userManager.Users.Where(x =>
                x.FirstName.Contains(searchTerm) ||
                x.LastName.Contains(searchTerm) ||
                x.Email.Contains(searchTerm) ||
                x.PhoneNumber.Contains(searchTerm) ||
                x.FacebookAddress.Contains(searchTerm)).ToList();

            if (user != null)
                return UserMapping.GetUserSearchResponse(user);
            
            throw new KeyNotFoundException("No item found!");
        }


        public async Task<bool> UserExists(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id) + " is empty");

            return await _userManager.Users.AnyAsync(a => a.Id == id);
        }
    }
}
