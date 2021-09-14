using BetterDaysContactBook.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BetterDaysContactBook.Core.helper
{
    public interface ITokenGenerator
    {
        Task<string> GenerateToken(AppUser appUser);
    }
}