namespace Es_Log.Models
{
    public class GetErrorLogViewModel
    {
        public Guid? userId { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Controller { get; set; }
        public string? Method { get; set; }
        public string? Action { get; set; }
        public string? Operation { get; set; } = "Search";
        public int? Page { get; set; }
        public int? RowCount { get; set; }
        public string? IndexName { get; set; }
        public string? Services { get; set; }
        public int? ErrorCode { get; set; }
    }
}
