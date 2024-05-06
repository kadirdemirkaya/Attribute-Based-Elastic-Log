using Es_Log.Extensions;
using Es_Log.Options;
using Microsoft.Extensions.Configuration;
using Nest;

namespace Es_Log.Providers
{
    public class ElasticClientProvider
    {
        public ElasticClient ElasticClient { get; }
        public string ElasticSearchHost { get; set; }
        public ElasticClientProvider(IConfiguration _configuration)
        {
            ElasticSearchOptions elasticSearchOptions = _configuration.GetOptions<ElasticSearchOptions>("ElasticSearchOptions");
            ElasticSearchHost = elasticSearchOptions.ElasticSearchHost;
            ElasticClient = CreateClient();
        }


        private ElasticClient CreateClient()
        {
            var connectionSettings = new ConnectionSettings(new Uri(ElasticSearchHost))
                .DisablePing()
                .DisableDirectStreaming(true)
                .SniffOnStartup(false)
                .SniffOnConnectionFault(false);

            return new ElasticClient(connectionSettings);
        }

        public ElasticClient CreateClientWithIndex(string defaultIndex)
        {
            var connectionSettings = new ConnectionSettings(new Uri(ElasticSearchHost))
                .DisablePing()
                .SniffOnStartup(false)
                .SniffOnConnectionFault(false)
                .DefaultIndex(defaultIndex);

            return new ElasticClient(connectionSettings);
        }
    }
}
