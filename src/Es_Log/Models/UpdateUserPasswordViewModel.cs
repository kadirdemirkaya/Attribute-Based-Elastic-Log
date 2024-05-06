namespace Es_Log.Models
{
    public class UpdateUserPasswordViewModel
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
