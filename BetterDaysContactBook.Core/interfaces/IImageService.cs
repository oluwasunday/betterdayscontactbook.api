using BetterDaysContactBook.Models.DTOs;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BetterDaysContactBook.Core.helper
{
    public interface IImageService
    {
        Task<UploadResult> UploadAsync(IFormFile image);
    }
}