using Nest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElasticSearch.IServices
{
    public interface IElasticSearchRepository
    {
        void CreateIndexSettings<T>(string nameIndex = null) where T : class;
        Task<bool> SynchronizedDataFromDb<TModel>(IEnumerable<TModel> documents, string indexName = null) where TModel : class;
        bool DeleteIndex(string indexName);
        BoolQueryDescriptor<T> CombineBoolQueryDescriptors<T>(params BoolQueryDescriptor<T>[] queries) where T : class;
        ElasticClient GetConnection();
    }
}
