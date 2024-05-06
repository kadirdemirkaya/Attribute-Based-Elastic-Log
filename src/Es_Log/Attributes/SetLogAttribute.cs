namespace Es_Log.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class SetLogAttribute : Attribute
    {
        public string UserId { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string HttpType { get; set; }
        public DateTime PostDate { get; set; }
        public string OldData { get; set; }
        public string ModelType { get; set; }
        public string Operation { get; set; }
    }
}
