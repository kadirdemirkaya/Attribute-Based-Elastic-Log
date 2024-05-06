using Microsoft.AspNetCore.Identity;

namespace Es_Log.Models.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string? AboutMe { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ChangedAt { get; set; }
        public bool IsActive { get; set; } = true;


        public User()
        {

        }

        public User(Guid id, string name, string email, string aboutMe, string phoneNumber)
        {
            Id = id;
            UserName = name;
            Email = email;
            NormalizedEmail = email.ToUpper();
            AboutMe = aboutMe;
            PhoneNumber = phoneNumber;
        }

        public User(string name, string email, string aboutMe, string phoneNumber)
        {
            Id = Guid.NewGuid();
            UserName = name;
            Email = email;
            NormalizedEmail = email.ToUpper();
            AboutMe = aboutMe;
            PhoneNumber = phoneNumber;
        }

        public static User Create(Guid id, string name, string email, string aboutMe, string phoneNumber)
            => new(id, name, email, aboutMe, phoneNumber);

        public static User Create(string name, string email, string aboutMe, string phoneNumber)
            => new(name, email, aboutMe, phoneNumber);
    }
}
