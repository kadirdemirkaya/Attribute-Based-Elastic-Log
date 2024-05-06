using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Es_Log.Models.Entities
{
    public class ESLogDbContext : IdentityDbContext<User, Role, Guid>
    {
        public ESLogDbContext()
        {

        }

        public ESLogDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
