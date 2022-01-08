using Common.Helpers;
using Common.Utils;
using Common.ViewModels.Authentication;
using Elasticsearch.Net;
using ElasticSearch.IServices;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch.Services
{
    public class ES_ThanhVien : IES_ThanhVien
    {
        private readonly IElasticSearchRepository _elasticSearchRepository;

        public ES_ThanhVien(IElasticSearchRepository elasticSearchRepository)
        {
            _elasticSearchRepository = elasticSearchRepository;
        }

        public async Task<PaginationResult<UserESViewModel>> GetAll(IdentityRequest request)
        {
            if (request.PageIndex <= 0)
            {
                request.PageIndex = 1;
            }
            if (request.PageSize <= 0)
            {
                request.PageSize = int.MaxValue;
            }
            var result = new PaginationResult<UserESViewModel>();

            try
            {
                var elasticClient = _elasticSearchRepository.GetConnection();
                var instance = Activator.CreateInstance<UserESViewModel>();
                List<string> fields = new List<string>();
                var searchRequest = new SearchDescriptor<UserESViewModel>().Index(Constants.ElasticSearch.UserEs)
                    .From((request.PageIndex - 1) * request.PageSize).Size(request.PageSize);
                var boolQuery = new BoolQueryDescriptor<UserESViewModel>();

                boolQuery.Must(m =>
                {
                    var queryContainer = new QueryContainer();

                    #region Query string
                    if (!string.IsNullOrWhiteSpace(request.MaThanhVien))
                    {
                        queryContainer &= m.QueryString(qt => qt.Query(request.MaThanhVien).Fields(nameof(instance.MaThanhVien).ToCamelCase()).Type(TextQueryType.Phrase).DefaultOperator(Operator.And));
                    }
                    if (!string.IsNullOrWhiteSpace(request.TenThanhVien))
                    {
                        queryContainer &= m.QueryString(qt => qt.Query(request.TenThanhVien).Fields(nameof(instance.TenThanhVien).ToCamelCase()).Type(TextQueryType.Phrase).DefaultOperator(Operator.And));
                    }
                    if (!string.IsNullOrWhiteSpace(request.SoDienThoai))
                    {
                        queryContainer &= m.QueryString(qt => qt.Query(request.SoDienThoai).Fields(nameof(instance.SoDienThoai).ToCamelCase()).Type(TextQueryType.Phrase).DefaultOperator(Operator.And));
                    }
                    if (!string.IsNullOrWhiteSpace(request.DiaChi))
                    {
                        queryContainer &= m.QueryString(qt => qt.Query(request.DiaChi).Fields(nameof(instance.DiaChi).ToCamelCase()).Type(TextQueryType.Phrase).DefaultOperator(Operator.And));
                    }
                    if (!string.IsNullOrWhiteSpace(request.CMND))
                    {
                        queryContainer &= m.QueryString(qt => qt.Query(request.CMND).Fields(nameof(instance.CMND).ToCamelCase()).Type(TextQueryType.Phrase).DefaultOperator(Operator.And));
                    }
                    #endregion

                    #region Term
                    if (request.GioiTinh.HasValue)
                    {
                        queryContainer &= m.Term(qt => qt.Value(request.GioiTinh.Value.ToString()).Field(nameof(instance.GioiTinh).ToCamelCase()));
                    }
                    #endregion

                    return queryContainer;
                });

                var container = Query<UserESViewModel>.Bool(b => boolQuery);
                searchRequest = searchRequest.Query(q => container);
                searchRequest.Sort(s=>s.Descending(x=>x.CreateAt));
                var json = elasticClient.RequestResponseSerializer.SerializeToString(searchRequest);
                var response = await elasticClient.SearchAsync<UserESViewModel>(searchRequest);
                if (response.IsValid)
                {
                    return result.Page((List<UserESViewModel>)response.Documents, request.PageIndex, request.PageSize, response.Total);
                }
                else
                {
                    return result.Page(null, request.PageIndex, request.PageSize, 0);
                }
            }
            catch
            {
                return result.Page(null, request.PageIndex, request.PageSize, 0);

            }

        }

        public async Task<bool> DeleteById(string id)
        {
            var elasticClient = _elasticSearchRepository.GetConnection();
            var instance = Activator.CreateInstance<UserESViewModel>();
            var response = await elasticClient.DeleteByQueryAsync<UserESViewModel>(q => q
              .Query(rq => rq
                  .Term(m => m
                  .Value(id)
                  .Field("_id")
              )).Index(Constants.ElasticSearch.UserEs).Refresh(true));
            return response.IsValid;
        }
        public async Task<bool> Insert(UserESViewModel document)
        {
            try
            {
                var elasticClient = _elasticSearchRepository.GetConnection();
                var entity = Activator.CreateInstance<UserESViewModel>();
                var index = await elasticClient.IndexAsync<UserESViewModel>(document, i => i
                     .Index(Constants.ElasticSearch.UserEs)
                     .Id(document.GetType().GetProperty(nameof(UserESViewModel.Id)).GetValue(document, null).ToString())
                     .Refresh(Refresh.True)
                    );
                var json = elasticClient.RequestResponseSerializer.SerializeToString(index);
                return index.IsValid;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> Update(string id, dynamic updateDocument)
        {
            try
            {
                var elasticClient = _elasticSearchRepository.GetConnection();
                var instance = Activator.CreateInstance<UserESViewModel>();
                var response = await elasticClient.UpdateAsync<UserESViewModel, UserESViewModel>(id, 
                    u => u.Index(Constants.ElasticSearch.UserEs).Doc(updateDocument).Refresh(Refresh.True));
                return response.IsValid;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SynchronizedData(IEnumerable<UserESViewModel> documents)
        {
            var elasticClient = _elasticSearchRepository.GetConnection();

            _elasticSearchRepository.CreateIndexSettings<UserESViewModel>(Constants.ElasticSearch.UserEs);
            var bulkIndexer = new BulkDescriptor();
            var stringId = Constants.ElasticSearch.UserEs;
            foreach (var document in documents)
            {
                bulkIndexer.Index<UserESViewModel>(i => i
                    .Document((UserESViewModel)Convert.ChangeType(document, typeof(UserESViewModel)))
                    .Id(document.GetType().GetProperty(nameof(UserESViewModel.Id)).GetValue(document, null).ToString())
                    .Index(stringId)
                    );
            }
            var index = await elasticClient.BulkAsync(bulkIndexer.Refresh(Refresh.True));
            return index.IsValid;
        }
        public bool DeleteIndex()
        {
            return _elasticSearchRepository.DeleteIndex(Constants.ElasticSearch.UserEs);
        }
    }
}
