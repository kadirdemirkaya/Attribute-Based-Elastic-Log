using Es_Log.Constants;
using Microsoft.AspNetCore.Identity;

namespace Es_Log.Models.Entities
{
    public class ESLogSeedContext(UserManager<User> _userManager)
    {
        public static async Task SeedAsync(ESLogDbContext context)
        {
            if (!context.Roles.Any())
            {
                var roles = new List<Role>()
                {
                    new Role(){Name = Constant.Role.User,NormalizedName = Constant.Role.User.ToUpper()},
                    new Role(){Name = Constant.Role.Admin,NormalizedName = Constant.Role.Admin.ToUpper()},
                    new Role(){Name = Constant.Role.Moderator,NormalizedName = Constant.Role.Moderator.ToUpper()},
                    new Role(){Name = Constant.Role.Guest,NormalizedName = Constant.Role.Guest.ToUpper()}
                };
                await context.Roles.AddRangeAsync(roles);
                await context.SaveChangesAsync();
            }
        }
    }
}
