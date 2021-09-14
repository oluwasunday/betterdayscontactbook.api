using BetterDaysContactBook.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace BetterDaysContactBook.Database
{
    public class BetterDaysContactBookContext : IdentityDbContext<AppUser>
    {
        public BetterDaysContactBookContext(DbContextOptions<BetterDaysContactBookContext> options) : base(options)
        {

        }

        public DbSet<AppUser> AppUsers { get; set; } 
    }
}
