using Common.Utils;
using Domain;
using Domain.Models;
using Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Hangfire;
using System.Threading.Tasks;
using System;
using Common.ViewModels.Store;
using Common.Helpers;
using AutoMapper;
using System.Linq;
using MongoDB.Bson;
using System.Text.RegularExpressions;

namespace Application.StoreServices
{
    public class StoreService : BasePrincipal, IStoreService
    {
        private readonly IMongoRepository _repository;
        private readonly IMapper _mapper;

        public StoreService(IHttpContextAccessor httpContextAccessor, IMongoRepository repository, IMapper mapper) : base(httpContextAccessor)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> AddOrUpdate(StoreViewModel model)
        {
            if (!_permission.checkAdmin())
                return Unauthorized(Constants.CodeError.Unauthorized, Constants.MessageResponse.Unauthorized);

            var thanhVien = await _repository.FirsOfDefaultAsync<ThanhVien>(x => x.Id == model.ThanhVienId);
            if (thanhVien == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);

            if (model.Id == null || model.Id == Guid.Empty)
            {
                var entity = _mapper.Map<CuaHang>(model);
                entity.TenThanhVien = thanhVien.TenThanhVien;
                await _repository.AddAsync<CuaHang>(entity);
                return Created(entity);
            }
            else
            {
                var entity = await _repository.GetById<CuaHang>(model.Id.ToString());
                if (entity == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);

                if (entity.TenCuaHang != model.TenCuaHang)
                {
                    BackgroundJob.Enqueue(() => UpdateProduct(model.TenCuaHang, entity.Id));
                }
                entity = entity.CuaHang(model);
                await _repository.UpdateAsnyc<CuaHang>(entity.Id.ToString(), entity);
                return Ok(entity);
            }
        }

        public async Task<ServiceResponse> GetById(Guid id)
        {
            var entity = await _repository.GetById<CuaHang>(id.ToString());
            if (entity == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);
            return Ok(entity);
        }

        public async Task<ServiceResponse> Delete(Guid id)
        {
            if (!_permission.checkAdmin())
                return Unauthorized(Constants.CodeError.Unauthorized, Constants.MessageResponse.Unauthorized);

            var entity = await _repository.GetById<CuaHang>(id.ToString());
            if (entity == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);
            BackgroundJob.Enqueue(() => DeleteLogicProduc(id));
            entity.IsDeleted = true;
            await _repository.UpdateAsnyc<CuaHang>(id.ToString(), entity);
            return Ok(entity);
        }

        public async Task<ServiceResponse> GetAll(StoreRequest request)
        {
            if(request.PageIndex <= 0)
            {
                request.PageIndex = 1;
            }
            if(request.PageSize <= 0)
            {
                request.PageSize = 10;
            }
            var query = new BsonDocument();
            
            if (!string.IsNullOrWhiteSpace(request.TenCuaHang))
            {
                query.Add(nameof(CuaHang.TenCuaHangKd), BsonRegularExpression.Create(new Regex(request.TenCuaHang.ConvertToUnSign())));
            }
            if (!string.IsNullOrWhiteSpace(request.MoTa))
            {
                query.Add(nameof(CuaHang.MoTaKd), BsonRegularExpression.Create(new Regex(request.MoTa.ConvertToUnSign())));
            }
            if (!string.IsNullOrWhiteSpace(request.TenThanhVien))
            {
                query.Add(nameof(CuaHang.TenThanhVienKd), BsonRegularExpression.Create(new Regex(request.TenThanhVien.ConvertToUnSign())));
            }
            if (!string.IsNullOrWhiteSpace(request.SoDienThoai))
            {
                query.Add(nameof(CuaHang.SoDienThoai), BsonRegularExpression.Create(new Regex(request.SoDienThoai)));
            }

            var data = _repository.FindForPageAsync<CuaHang>(query, request.PageIndex, request.PageSize);
            var count = _repository.CountAsync<CuaHang>(query);
            await Task.WhenAll(data, count);
            var result = new PaginationResult<CuaHang>();
            return Ok(result.Page(data.Result, request.PageIndex, request.PageSize, count.Result));
        }

        public async Task<ServiceResponse> GetAllForUser(StoreRequest request)
        {
            if (request.PageIndex <= 0)
            {
                request.PageIndex = 1;
            }
            if (request.PageSize <= 0)
            {
                request.PageSize = 10;
            }

            var query = new BsonDocument();

            query.Add(nameof(CuaHang.ThanhVienId), _userId.ToString());

            if (!string.IsNullOrWhiteSpace(request.TenCuaHang))
            {
                query.Add(nameof(CuaHang.TenCuaHangKd), BsonRegularExpression.Create(new Regex(request.TenCuaHang.ConvertToUnSign())));
            }
            if (!string.IsNullOrWhiteSpace(request.MoTa))
            {
                query.Add(nameof(CuaHang.MoTaKd), BsonRegularExpression.Create(new Regex(request.MoTa.ConvertToUnSign())));
            }
            if (!string.IsNullOrWhiteSpace(request.TenThanhVien))
            {
                query.Add(nameof(CuaHang.TenThanhVienKd), BsonRegularExpression.Create(new Regex(request.TenThanhVien.ConvertToUnSign())));
            }
            if (!string.IsNullOrWhiteSpace(request.SoDienThoai))
            {
                query.Add(nameof(CuaHang.SoDienThoai), BsonRegularExpression.Create(new Regex(request.SoDienThoai)));
            }

            var data = _repository.FindForPageAsync<CuaHang>(query, request.PageIndex, request.PageSize);
            var count = _repository.CountAsync<CuaHang>(query);
            await Task.WhenAll(data, count);
            var result = new PaginationResult<CuaHang>();
            return Ok(result.Page(data.Result, request.PageIndex, request.PageSize, count.Result));
        }

        #region BackJob
        public async Task UpdateProduct(string tenCuaHang, Guid cuaHangId)
        {
            var sanPhams = await _repository.GetAllAsync<SanPham>(x => x.CuaHangId == cuaHangId);
            if (sanPhams != null && sanPhams.Any())
            {
                foreach (var item in sanPhams)
                {
                    item.TenCuaHang = tenCuaHang;
                    await _repository.UpdateAsnyc<SanPham>(item.Id.ToString(), item);
                }
            }
        }

        public async Task DeleteLogicProduc(Guid cuaHangId)
        {
            var sanPhams = await _repository.GetAllAsync<SanPham>(x => x.CuaHangId == cuaHangId);
            if (sanPhams != null && sanPhams.Any())
            {
                foreach (var item in sanPhams)
                {
                    item.IsDeleted = true;
                    await _repository.UpdateAsnyc<SanPham>(item.Id.ToString(), item);
                }
            }
        }
        #endregion
    }
}
