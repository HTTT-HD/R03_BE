using Common.Helpers;
using Elasticsearch.Net;
using Nest;
using System;

namespace ElasticSearch.Services
{
    public static class ElasticClientConnection
    {
        private static string _elasticUrl = Constants.ElasticSearch.URL;
        const int REQUEST_TIMEOUT = 5; //default => 5 seconds

        public static ElasticClient Connect()
        {
            ConnectionSettings connectionSettings;
            ElasticClient elasticClient;
            StaticConnectionPool connectionPool;
            var nodes = new Uri[]
            {
                new Uri(_elasticUrl),
            };

            connectionPool = new StaticConnectionPool(nodes);
            connectionSettings = new ConnectionSettings(connectionPool).RequestTimeout(TimeSpan.FromSeconds(REQUEST_TIMEOUT));
            elasticClient = new ElasticClient(connectionSettings);

            return elasticClient;
        }
    }
}
