using Microsoft.AspNetCore.Identity;

namespace Es_Log.Models.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ChangedAt { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
