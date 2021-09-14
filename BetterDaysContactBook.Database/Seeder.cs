using BetterDaysContactBook.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BetterDaysContactBook.Database
{
    public class Seeder
    {
        public static async Task<bool> SeedContacts(BetterDaysContactBookContext dbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                await dbContext.Database.EnsureCreatedAsync();
                if (!dbContext.Users.Any())
                {
                    List<string> roles = new List<string> { "Admin", "Regular" };

                    foreach (var role in roles)
                    {
                        await roleManager.CreateAsync(new IdentityRole { Name = role });
                    }

                    List<AppUser> users = new List<AppUser>
                    {
                        new AppUser
                        {
                            FirstName = "Ola",
                            LastName = "Dominion",
                            Email = "dominionkoncept01@gmail.com",
                            PhoneNumber = "08054348433",
                            Address = "Oba Erinwole Rd, Makun, Sagamu, Ogun",
                            Gender = "Female",
                            DateOfBirth = DateTime.Now,
                            PhotoUrl = "default.jpg",
                            UserName = "gratefuldominion"
                        },
                        new AppUser
                        {
                            FirstName = "Bimbo",
                            LastName = "Onas",
                            Email = "abim@gmail.com",
                            PhoneNumber = "07064543424",
                            Address = "Olaoluwa, Ibadan, Oyo",
                            Gender = "Female",
                            DateOfBirth = new DateTime(2009-09-06),
                            PhotoUrl = "default.jpg",
                            UserName = "onasabim"
                        },
                        new AppUser
                        {
                            FirstName = "Mokore",
                            LastName = "Beulah",
                            Email = "mokore@gmail.com",
                            PhoneNumber = "08199889990",
                            Address = "Alafara, Ologuneru, Ibadan, Oyo",
                            Gender = "Male",
                            DateOfBirth = new DateTime(2009-09-06),
                            PhotoUrl = "default.jpg",
                            UserName = "mokorebeulah"
                        }
                    };

                    foreach (var user in users)
                    {
                        await userManager.CreateAsync(user, "Password@123");
                        if (user == users[0])
                        {
                            await userManager.AddToRoleAsync(user, "Admin");
                        }
                        else
                            await userManager.AddToRoleAsync(user, "Regular");
                    }




                    var path = File.ReadAllText(baseDir + @"/jsons/contacts.json");

                    var contactBook = JsonConvert.DeserializeObject<List<AppUser>>(path);
                    await dbContext.AppUsers.AddRangeAsync(contactBook);
                }
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new AccessViolationException($"Error occur while accessing the database: {ex}");
            }
        }
    }
}
