using Common.Utils;
using Common.ViewModels.Authentication;
using Common.ViewModels.Permission;
using Common.ViewModels.Role;
using System;
using System.Threading.Tasks;

namespace Application.RoleServices
{
    public interface IRoleService
    {
        Task<ServiceResponse> AddOrUpdate(RoleViewModel model);
        Task<ServiceResponse> GetById(Guid id);
        Task<ServiceResponse> DeleteAsnyc(Guid id);
        Task<ServiceResponse> GetAll(RoleRequest request);
        Task<ServiceResponse> AddPermission(AddPermisisonToRole request);
        Task<ServiceResponse> AddUser(AddUserToRole request);
        Task<ServiceResponse> GetPermissionInRole(PermissionRequest request);
        Task<ServiceResponse> GetUserInRole(UserInRole request);
        Task<ServiceResponse> GetUserOutRole(UserInRole request);
    }
}
