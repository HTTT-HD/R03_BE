using Common.Utils;
using Common.ViewModels.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElasticSearch.IServices
{
    public interface IES_ThanhVien
    {
        bool DeleteIndex();
        Task<bool> DeleteById(string id);
        Task<bool> Insert(UserESViewModel document);
        Task<bool> Update(string id, dynamic updateDocument);
        Task<bool> SynchronizedData(IEnumerable<UserESViewModel> documents);
        Task<PaginationResult<UserESViewModel>> GetAll(IdentityRequest request);
    }
}
