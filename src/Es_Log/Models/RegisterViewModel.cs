using System.ComponentModel.DataAnnotations;

namespace Es_Log.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string? AboutMe { get; set; }

        [StringLength(10)]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
