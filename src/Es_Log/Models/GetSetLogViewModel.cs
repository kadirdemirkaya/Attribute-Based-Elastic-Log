namespace Es_Log.Models
{
    public class GetSetLogViewModel
    {
        public Guid? userId { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Controller { get; set; }
        public string? Action { get; set; }
        public string? ModelType { get; set; }
        public string? Operation { get; set; } = "Search";
        public int? Page { get; set; }
        public int? RowCount { get; set; }
        public string? IndexName { get; set; }
    }
}
