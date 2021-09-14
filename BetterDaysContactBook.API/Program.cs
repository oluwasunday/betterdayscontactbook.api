using BetterDaysContactBook.Database;
using BetterDaysContactBook.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterDaysContactBook.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            //CreateHostBuilder(args).Build().Run();

            using (var servicScope = host.Services.CreateScope())
            {
                var services = servicScope.ServiceProvider;
                try
                {

                    BetterDaysContactBookContext context = services.GetRequiredService<BetterDaysContactBookContext>();
                    UserManager<AppUser> userManager = services.GetRequiredService<UserManager<AppUser>>();
                    RoleManager<IdentityRole> roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    Seeder.SeedContacts(context, userManager, roleManager).Wait();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
