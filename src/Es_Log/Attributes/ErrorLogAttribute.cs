namespace Es_Log.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ErrorLogAttribute : Attribute
    {
        public string UserId { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string HttpType { get; set; }
        public DateTime PostDate { get; set; }
        public int UserID { get; set; }
        public string Message { get; set; }
        public string? Service { get; set; }
        public int ErrorCode { get; set; }
    }
}
