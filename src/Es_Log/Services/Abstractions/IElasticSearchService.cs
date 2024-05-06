using Es_Log.Models.Logs;

namespace Es_Log.Services.Abstractions
{
    public interface IElasticSearchService<T> where T : class
    {
        public void CheckExistsAndInsertLog(T logModel, string indexName);
        public IReadOnlyCollection<RequestLogModel> SearchRequestLog(Guid? userID, DateTime? BeginDate, DateTime? EndDate, string controller = "", string operation = "Search", string action = "", int? page = 0, int? rowCount = 10, string? indexName = "request_log");

        public IReadOnlyCollection<ErrorLogModel> SearchErrorLog(Guid? userID, int? errorCode, DateTime? BeginDate, DateTime? EndDate, string controller = "", string operation = "Error", string action = "", string method = "", string services = "", int page = 0, int rowCount = 10, string indexName = "error_log");

        public IReadOnlyCollection<SetLogModel> SearchSetLog(Guid? userID, DateTime? BeginDate, DateTime? EndDate, string? controller, string modelType = "", string operation = "Set", int page = 0, int rowCount = 10, string indexName = "set_log");
    }
}
