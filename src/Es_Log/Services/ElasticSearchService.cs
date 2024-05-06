using Es_Log.Services.Abstractions;
using Es_Log.Providers;
using Nest;
using Es_Log.Models.Logs;

namespace Es_Log.Services
{
    public class ElasticSearchService<T>(ElasticClientProvider _provider) : IElasticSearchService<T> where T : class
    {
        private ElasticClient _client = _provider.ElasticClient;

        public void CheckExistsAndInsertLog(T logModel, string indexName)
        {
            if (!_client.Indices.Exists(indexName).Exists)
            {
                var newIndexName = indexName + System.DateTime.Now.Ticks;

                var indexSettings = new IndexSettings();
                indexSettings.NumberOfReplicas = 1;
                indexSettings.NumberOfShards = 3;

                var response = _client.Indices.Create(newIndexName, index =>
                   index.Map<T>(m => m.AutoMap()
                          )
                  .InitializeUsing(new IndexState() { Settings = indexSettings })
                  .Aliases(a => a.Alias(indexName)));

            }
            IndexResponse responseIndex = _client.Index<T>(logModel, idx => idx.Index(indexName));
            int a = 0;
        }

        public IReadOnlyCollection<ErrorLogModel> SearchErrorLog(Guid? userID, int? errorCode, DateTime? BeginDate, DateTime? EndDate, string controller = "", string operation = "Search", string action = "", string method = "", string services = "", int page = 0, int rowCount = 10, string indexName = "error_log")
        {
            BeginDate = BeginDate == null ? DateTime.Parse("01/01/1900") : BeginDate;
            EndDate = EndDate == null ? DateTime.Now : EndDate;

            //Değişecek !
            var response = _client.Search<ErrorLogModel>(s => s
            .From(page)
            .Size(rowCount)
            .Sort(ss => ss.Descending(p => p.PostDate))
            .Query(q => q
                .Bool(b => b
                    .Must(
                        q => q.Term(t => t.UserId, userID),
                        q => q.Term(t => t.Controller, controller.ToLower().Trim()),
                        q => q.Term(t => t.Action, action.ToLower().Trim()),
                        q => q.Term(t => t.ErrorCode, errorCode),
                        q => q.Term(t => t.Service, services.ToLower().Trim()),
                        q => q.DateRange(dr => dr
                        .Field(p => p.PostDate)
                        .GreaterThanOrEquals(DateMath.Anchored(((DateTime)BeginDate).AddDays(-1)))
                        .LessThanOrEquals(DateMath.Anchored(((DateTime)EndDate).AddDays(1)))
                        ))
                     )
                  )
            .Index(indexName)
            );
            return response.Documents;
        }

        public IReadOnlyCollection<RequestLogModel> SearchRequestLog(Guid? userID, DateTime? BeginDate, DateTime? EndDate, string controller = "", string operation = "Error", string action = "", int? page = 0, int? rowCount = 10, string? indexName = "request_log")
        {
            BeginDate = BeginDate == null ? DateTime.Parse("01/01/1900") : BeginDate;
            EndDate = EndDate == null ? DateTime.Now : EndDate;

            var response = _client.Search<RequestLogModel>(s => s
                .From(page)
                .Size(rowCount)
                .Sort(ss => ss.Descending(p => p.PostDate))
                .Query(q => q
                    .Bool(b =>
                    {
                        var must = new List<Func<QueryContainerDescriptor<RequestLogModel>, QueryContainer>>();

                        if (userID != Guid.Empty)
                            must.Add(mq => mq.Term(t => t.UserId, userID.ToString().ToLower().Trim()));
                        if (!string.IsNullOrWhiteSpace(controller))
                            must.Add(mq => mq.Term(t => t.Controller, controller.ToLower().Trim()));
                        if (!string.IsNullOrWhiteSpace(action))
                            must.Add(mq => mq.Term(t => t.Action, action.ToLower().Trim()));
                        if (BeginDate != null && EndDate != null)
                            must.Add(mq => mq.DateRange(dr => dr
                                .Field(p => p.PostDate)
                                .GreaterThanOrEquals(DateMath.Anchored(((DateTime)BeginDate).AddDays(-1)))
                                .LessThanOrEquals(DateMath.Anchored(((DateTime)EndDate).AddDays(1)))
                            ));

                        return b.Must(must);
                    })
                )
                .Index(indexName)
            );

            return response.Documents;
        }

        public IReadOnlyCollection<SetLogModel> SearchSetLog(Guid? userID, DateTime? BeginDate, DateTime? EndDate, string? controller, string modelType = "", string operation = "Set", int page = 0, int rowCount = 10, string indexName = "set_log")
        {
            BeginDate = BeginDate == null ? DateTime.Parse("01/01/1900") : BeginDate;
            EndDate = EndDate == null ? DateTime.Now : EndDate;
            //var response = _client.Search<SetLogModel>(s => s
            //   .From(page)
            //   .Size(rowCount)
            //   .Sort(ss => ss.Descending(p => p.PostDate))
            //   .Query(q => q
            //       .Bool(b => b
            //           .Must(
            //               q => q.Term(t => t.UserId, userID),
            //               q => q.Term(t => t.Operation, operation.ToLower().Trim()),
            //               q => q.Term(t => t.Controller, controller.ToLower().Trim()),
            //               q => q.DateRange(dr => dr
            //               .Field(p => p.PostDate)
            //               .GreaterThanOrEquals(DateMath.Anchored(((DateTime)BeginDate).AddDays(-1)))
            //               .LessThanOrEquals(DateMath.Anchored(((DateTime)EndDate).AddDays(1)))
            //               ))
            //            )
            //         )
            //        .Index(indexName)
            //   );

            var response = _client.Search<SetLogModel>(s => s
                 .From(page)
                 .Size(rowCount)
                 .Sort(ss => ss.Descending(p => p.PostDate))
                 .Query(q => q
                     .Bool(b => b
                         .Must(must =>
                         {
                             // Here we use a list to accumulate all query conditions
                             var queryConditions = new List<QueryContainer>();

                             // Adding conditions based on your parameters
                             if (userID != Guid.Empty)
                                 queryConditions.Add(must.Term(t => t.UserId, userID.ToString().ToLower().Trim()));
                             if (!string.IsNullOrWhiteSpace(operation))
                                 queryConditions.Add(must.Term(t => t.Operation, operation.ToLower().Trim()));
                             if (!string.IsNullOrWhiteSpace(controller))
                                 queryConditions.Add(must.Term(t => t.Controller, controller.ToLower().Trim()));
                             if (BeginDate != null && EndDate != null)
                                 queryConditions.Add(must.DateRange(dr => dr
                                     .Field(p => p.PostDate)
                                     .GreaterThanOrEquals(DateMath.Anchored(((DateTime)BeginDate).AddDays(-1)))
                                     .LessThanOrEquals(DateMath.Anchored(((DateTime)EndDate).AddDays(1)))
                                 ));

                             // Use the static Query<>.Bool to combine all query conditions
                             return must.Bool(b => b.Must(queryConditions.ToArray()));
                         })
                     )
                 )
                 .Index(indexName)
             );
            return response.Documents;
        }
    }
}
