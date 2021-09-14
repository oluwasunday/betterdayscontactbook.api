using BetterDaysContactBook.Models.DTOs;
using System.Threading.Tasks;

namespace BetterDaysContactBook.Core
{
    public interface IAuthentication
    {
        Task<UserDTO> Login(LoginDTO loginRequest);
        Task<UserDTO> Register(RegisterDTO registerRequest);
    }
}