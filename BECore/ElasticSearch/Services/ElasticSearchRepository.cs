using Elasticsearch.Net;
using Nest;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

using System;
using Common.Helpers;
using ElasticSearch.IServices;

namespace ElasticSearch.Services
{
    public class ElasticSearchRepository : IElasticSearchRepository
    {
        public ElasticClient elasticClient;

        public ElasticClient GetConnection()
        {
            try
            {
                if (elasticClient == null)
                {
                    elasticClient = ElasticClientConnection.Connect();
                }
            }
            catch
            {
                elasticClient = null;
            }
            return elasticClient;
        }

        public async Task<bool> SynchronizedDataFromDb<TModel>(IEnumerable<TModel> documents, string indexName = null) where TModel : class
        {
            CreateIndexSettings<TModel>(indexName);
            var bulkIndexer = new BulkDescriptor();
            var stringId = indexName is null ? typeof(TModel).Name.ToLower() : indexName.ToLower();
            foreach (var document in documents)
            {
                bulkIndexer.Index<TModel>(i => i
                    .Document((TModel)Convert.ChangeType(document, typeof(TModel)))
                    .Index(stringId)
                    );
            }
            var index = await elasticClient.BulkAsync(bulkIndexer.Refresh(Refresh.True));
            return index.IsValid;
        }

        public bool DeleteIndex(string indexName)
        {
            try
            {
                GetConnection();
                var response = elasticClient.Indices.Delete(indexName);

                return response.IsValid;
            }
            catch
            {
                return false;
            }
        }

        public void CreateIndexSettings<T>(string nameIndex = null) where T : class
        {
            GetConnection();

            var instance = Activator.CreateInstance<T>();
            var indexName = nameIndex is null ? instance.GetType().Name.ToLower() : nameIndex.ToLower();
            if (elasticClient.Indices.Exists(indexName).Exists)
            {
                return;
            }
            var filterAnalyzer = new List<string>()
                       {
                        "lowercase", "asciifolding",
                       };
            CustomAnalyzer customAnlyzer = new CustomAnalyzer
            {
                Tokenizer = "mynGram",
                Filter = filterAnalyzer,
            };


            var propertiesDescriptor = new PropertiesDescriptor<T>();

            foreach (var prop in typeof(T).GetProperties())
            {
                if (!(prop.PropertyType == typeof(string)))
                {
                    continue;
                }
                var textPropertyDescriptor = new TextPropertyDescriptor<T>();
                propertiesDescriptor.Text(t =>
                textPropertyDescriptor
                        .Name(prop.Name.ToCamelCase())
                        .Analyzer("analyzer_userdefind")
                        .SearchAnalyzer("analyzer_userdefind")
                );
            }

            var typeMappingDescriptor = new TypeMappingDescriptor<T>()
                .AutoMap()
                .Properties(p => propertiesDescriptor);

            var indexDescriptor = new CreateIndexDescriptor(indexName)
                .Settings(s => s
                            .Analysis(a => a
                                .Tokenizers(tz => tz
                                    .NGram("mynGram", ng => ng
                                         .MinGram(1)
                                         .MaxGram(2)
                                    )
                                )
                                 .Analyzers(an => an
                                    .UserDefined("analyzer_userdefind", customAnlyzer)
                                )
                            )
                            .Setting("index.max_result_window", 2147483647)
                        )
            .Map(m => m
                .AutoMap());

            var index = elasticClient.Indices.Create(indexName, c => indexDescriptor
                        .Map<T>(mm => typeMappingDescriptor)
                    );
            var res = index.IsValid;
            return;
        }

        public BoolQueryDescriptor<T> CombineBoolQueryDescriptors<T>(params BoolQueryDescriptor<T>[] queries) where T : class
        {
            var descriptor = new BoolQueryDescriptor<T>();
            var combinedQuery = (IBoolQuery)descriptor;

            foreach (var query in queries.Cast<IBoolQuery>())
            {
                if (query.Must != null)
                {
                    combinedQuery.Must = combinedQuery.Must != null
                        ? combinedQuery.Must.Concat(query.Must)
                        : (query.Must.ToArray());
                }
                if (query.Should != null)
                {
                    combinedQuery.Should = combinedQuery.Should != null
                        ? combinedQuery.Should.Concat(query.Should)
                        : (query.Should.ToArray());
                }

                if (query.MustNot != null)
                {
                    combinedQuery.MustNot = combinedQuery.MustNot != null
                        ? combinedQuery.MustNot.Concat(query.MustNot)
                        : (query.MustNot.ToArray());
                }

                if (query.Filter != null)
                {
                    combinedQuery.Filter = combinedQuery.Filter != null
                        ? combinedQuery.Filter.Concat(query.Filter)
                        : (query.Filter.ToArray());
                }
            }

            return descriptor;
        }
    }
}
