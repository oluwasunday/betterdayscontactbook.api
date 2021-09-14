using BetterDaysContactBook.Models;
using BetterDaysContactBook.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterDaysContactBook.Core
{
    public interface IContactBookRepository
    {
        void AddUser(AppUser user);
        void DeleteUser(AppUser user);
        IEnumerable<AppUser> GetAllUsers();
        AppUser GetUser(string id);
        AppUser GetUserByEmail(string email);
        AppUser GetUserByFirstName(string firstName);
        void UpdateUser(string id, AppUser user);
        void UpdateUserPhoto(string id, string photo);
        bool UserExists(string id);
        UserDTO SearchUsersByTerm(string term);
    }
}
