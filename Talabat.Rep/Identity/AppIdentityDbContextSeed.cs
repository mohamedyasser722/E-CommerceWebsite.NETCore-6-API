using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            // Seed users
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Mohamed Yasser",
                    Email = "mohamedyasser722@gmail.com",
                    UserName = "mohamedyasser722",
                    PhoneNumber = "+20 1210078661"
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}
