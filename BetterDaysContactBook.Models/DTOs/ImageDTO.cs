using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterDaysContactBook.Models.DTOs
{
    public class ImageDTO
    {
        public IFormFile Image { get; set; } 
    }
}
