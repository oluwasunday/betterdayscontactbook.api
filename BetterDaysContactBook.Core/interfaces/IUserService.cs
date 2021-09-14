using BetterDaysContactBook.Core.helper;
using BetterDaysContactBook.Models;
using BetterDaysContactBook.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetterDaysContactBook.Core
{
    public interface IUserService
    {
        Task<bool> DeleteUser(string userId);
        Task<UserDTO> GetUserById(string id);
        Task<UserDTO> GetUserByEmail(string email);
        Task<bool> Update(string userId, UpdateUserDTO updateUser);
        Task<bool> UpdatePhotoUrl(string newPhotoUrl);
        Task<UserDTO> AddNewUser(RegisterDTO registerRequest);
        Task<PagedList<UserDTO>> GetAllUsers(PagingParams paging);
        List<UserDTO> SearchUsersByTerm(string searchTerm);
        Task<bool> UserExists(string id);
    }
}