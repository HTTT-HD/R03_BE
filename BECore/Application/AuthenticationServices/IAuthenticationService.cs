using Common.Utils;
using Common.ViewModels.Authentication;
using System;
using System.Threading.Tasks;

namespace Application.AuthenticationServices
{
    public interface IAuthenticationService
    {
        Task<ServiceResponse> CreateAsync(IdentityViewModel model);
        Task<ServiceResponse> UpdateAsync(IdentityUpdate model);
        Task<ServiceResponse> GetAll(IdentityRequest request);
        Task<ServiceResponse> GetById(Guid id);
        Task<ServiceResponse> GetUserLogin();
        Task<ServiceResponse> DeleteAsnyc(Guid id);
        Task<ServiceResponse> LoginAction(LoginViewModel model);
    }
}
