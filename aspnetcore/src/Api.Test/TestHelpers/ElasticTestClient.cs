using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;

namespace Api.Test.TestHelpers
{
    public class ElasticTestClient
    {
        public static ElasticClient Create(Action<IApiCallDetails> handler, List<string>? indices = null)
        {
            var connection = new MockElasticConnection(indices);
            var connectionPool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));
            var settings = new ConnectionSettings(connectionPool, connection);
            settings.OnRequestCompleted(handler).EnableDebugMode().DisableDirectStreaming();
            return new ElasticClient(settings);
        }
    }
}
