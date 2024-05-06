namespace Es_Log.Options
{
    public class SqlServerOptions
    {
        public string SqlConnection { get; set; }
        public string RetryCount { get; set; }

        public string RetryDelay { get; set; }
    }
}
