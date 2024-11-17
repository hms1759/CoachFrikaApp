using coachfrikaaaa.APIs.Entity;
using Microsoft.AspNetCore.Identity;

namespace CoachFrika.Common.AppUser
{
    public static class AppRoles
    {
        public const string Coach = "Coach";
        public const string Teacher = "Teacher";
        public const string Admin = "Admin";
        public const string SuperAdmin = "SuperAdmin";
    }

    public class SeedRoles
    {
        public static async Task Initialize(RoleManager<IdentityRole> roleManager)
        {

            var roles = new[] { AppRoles.Coach, AppRoles.Teacher, AppRoles.Admin, AppRoles.SuperAdmin };

            foreach (var role in roles)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }

    public class SeedUsers
    {
        public static async Task Initialize(UserManager<CoachFrikaUsers> userManager)
        {
            var defaultUser = await userManager.FindByEmailAsync("admin@admin.com");

            if (defaultUser == null)
            {
                var user = new CoachFrikaUsers
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    FullName = "Admin User",
                    Role = 1
                };

                var result = await userManager.CreateAsync(user, "AdminP@ssw0rd");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, AppRoles.SuperAdmin); // Assign Role 
                }
            }
        }
    }

}
