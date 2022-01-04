using Common.Helpers;
using Common.Utils;
using Common.ViewModels.Permission;
using Domain;
using Domain.Models;
using Domain.Repositories;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.PermissionServices
{
    public class PermissionService : BasePrincipal, IPermissionService
    {
        private readonly IMongoRepository _repository;

        public PermissionService(IHttpContextAccessor httpContextAccessor, IMongoRepository repository) : base(httpContextAccessor)
        {
            _repository=repository;
        }

        public async Task<ServiceResponse> AddPermissionDefault()
        {
            var permissions = PermissionDefault();
            var newPermissions = new List<Quyen>();

            foreach (var permission in permissions)
            {
                var check = await _repository.AnyLiqnAsync<Quyen>(x => x.Id == permission.Id);
                if (!check)
                {
                    newPermissions.Add(permission);
                }
            }
            if(newPermissions != null && newPermissions.Any())
            {
                await _repository.AddRangeAsync<Quyen>(newPermissions);
            }
            return Ok(true);
        }

        public async Task<ServiceResponse> GetAllPermisison(PermissionRequest request)
        {
            var query = new QueryDocument();
            if (!string.IsNullOrWhiteSpace(request.Ma))
            {
                query.Add(nameof(Quyen.Ma), BsonRegularExpression.Create(new Regex(request.Ma)));
            }
            if (!string.IsNullOrWhiteSpace(request.Ten))
            {
                query.Add(nameof(Quyen.Ten), BsonRegularExpression.Create(new Regex(request.Ten.ConvertToUnSign())));
            }

            return Ok(await _repository.FindAllAsync<Quyen>(query));
        }

        private List<Quyen> PermissionDefault()
        {
            var permissions = new List<Quyen>()
            {
                new Quyen()
                {
                    Id = Guid.Parse("F16BBCEC-4E1B-4432-BBED-30BC5266E061"),
                    Ma = Constants.Permission.Admin,
                    Ten = "Admin"
                },
                new Quyen()
                {
                    Id = Guid.Parse("E9D47F7F-575A-4CDA-BD48-A3B233E47CA4"),
                    Ma = Constants.Permission.NguoiBanHang,
                    Ten = "Người bán hàng"
                },
                new Quyen()
                {
                    Id = Guid.Parse("61145348-EA4F-4BA1-9AF7-AB836D34F1CD"),
                    Ma = Constants.Permission.NguoiGiaoHang,
                    Ten = "Người giao hàng"
                }
            };
            return permissions;
        }
    }
}
