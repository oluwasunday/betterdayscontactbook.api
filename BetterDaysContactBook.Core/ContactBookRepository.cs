using BetterDaysContactBook.Core.helper;
using BetterDaysContactBook.Database;
using BetterDaysContactBook.Models;
using BetterDaysContactBook.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterDaysContactBook.Core
{
    public class ContactBookRepository : IContactBookRepository
    {
        private readonly BetterDaysContactBookContext _context;

        public ContactBookRepository(BetterDaysContactBookContext context)
        {
            this._context = context;
        }

        public void AddUser(AppUser user)
        {

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Id = Guid.NewGuid().ToString();
            _context.Users.Add(user);
            _context.SaveChanges();
        }


        public void DeleteUser(AppUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
        }


        public AppUser GetUser(string id)
        {
            if (id == string.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return  _context.Users
              .FirstOrDefault(c => c.Id == id);
        }


        public AppUser GetUserByEmail(string email)
        {
            if (email == string.Empty)
            {
                throw new ArgumentNullException(nameof(email));
            }

            return _context.Users
              .FirstOrDefault(c => c.Email == email);
        }


        public AppUser GetUserByFirstName(string firstName)
        {
            if (firstName == string.Empty)
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            return _context.Users
              .FirstOrDefault(c => c.FirstName == firstName);
        }


        public AppUser GetUserByPhoneNumber(string id, string phoneNumber)
        {
            if (phoneNumber == string.Empty)
            {
                throw new ArgumentNullException(nameof(phoneNumber));
            }


            return _context.Users
              .FirstOrDefault(c => c.PhoneNumber == phoneNumber);
        }


        public IEnumerable<AppUser> GetAllUsers()
        {
            return _context.Users.ToList();
        }


        public IEnumerable<AppUser> GetAllUsers(string firstName)
        {
            return _context.Users.ToList();
        }


        public void UpdateUser(string id, AppUser user)
        {
            if (id == string.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var userr = _context.Users
                .Where(x => x.Id == id).FirstOrDefault();

            _context.Update(userr);
            _context.SaveChanges();
        }


        public void UpdateUserPhoto(string id, string photo)
        {
            if (id == string.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (string.IsNullOrEmpty(photo))
            {
                throw new ArgumentNullException(nameof(photo));
            }

            var user = _context.Users
                .Where(x => x.Id == id).FirstOrDefault();
            user.PhotoUrl = photo;

            _context.Update(user);
            _context.SaveChanges();
        }


        public bool UserExists(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            return _context.Users.Any(a => a.Id == id);
        }


        public List<AppUser> SearchUsersByTerm(string searchTerm)
        {
            var user = _context.AppUsers.Where(x =>
                x.FirstName.Contains(searchTerm) ||
                x.LastName.Contains(searchTerm) ||
                x.Email.Contains(searchTerm) ||
                x.PhoneNumber.Contains(searchTerm) ||
                x.FacebookAddress.Contains(searchTerm)).ToList();

            if(user != null)
            {
                //return UserMapping.GetUserSearchResponse(user);
                return user;
            }
            throw new KeyNotFoundException("No item found or Invalid search item");
        }

        UserDTO IContactBookRepository.SearchUsersByTerm(string term)
        {
            throw new NotImplementedException();
        }
    }
}
