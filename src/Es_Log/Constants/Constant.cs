namespace Es_Log.Constants
{
    public static class Constant
    {
        public const string RequestLog = "request_log";
        public const string ErrorLog = "error_log";
        public const string SetLog = "set_log";
        
        public static class Controller
        {
            public const string Auth = "Auth";
            public const string Role = "Role";
        }

        public static class Role
        {
            public const string User = "User";
            public const string Admin = "Admin";
            public const string Moderator = "Moderator";
            public const string Guest = "Guest";
        }
    }
}
