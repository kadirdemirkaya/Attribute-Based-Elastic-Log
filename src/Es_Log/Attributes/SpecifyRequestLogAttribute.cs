using Es_Log.Models.Enums;

namespace Es_Log.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class SpecifyRequestLogAttribute : Attribute
    {
        public string UserId { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string HttpType { get; set; }
        public DateTime PostDate { get; set; }
        public ActionType ActionType { get; set; }
        public string OldData { get; set; }
    }
}
