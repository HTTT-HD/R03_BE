using AutoMapper;

using Common.Helpers;
using Common.Utils;
using Common.ViewModels.Product;

using Domain;
using Domain.Models;
using Domain.Repositories;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.ProductServices
{
    public class ProductService : BasePrincipal, IProductService
    {
        private readonly IMongoRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(IHttpContextAccessor httpContextAccessor, IMongoRepository repository, IMapper mapper) : base(httpContextAccessor)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResponse> AddOrUpdate(ProductViewModel model)
        {
            var danhMuc = await _repository.GetById<DanhMuc>(model.DanhMucId.ToString());
            if(danhMuc == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);
            var cuaHang = await _repository.GetById<CuaHang>(model.CuaHangId.ToString());
            if(cuaHang == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);

            if(model.Id == null || model.Id == Guid.Empty)
            {
                var entity = _mapper.Map<SanPham>(model);
                entity.TenCuaHang = cuaHang.TenCuaHang;
                entity.TenDanhMuc = danhMuc.TenDanhMuc;
                await _repository.AddAsync<SanPham>(entity);
                return Ok(entity);
            }
            else
            {
                var entity = await _repository.GetById<SanPham>(model.Id.ToString());
                if(entity == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);

                entity = entity.SanPham(model);
                entity.TenCuaHang = cuaHang.TenCuaHang;
                entity.TenDanhMuc = danhMuc.TenDanhMuc;
                await _repository.UpdateAsnyc<SanPham>(entity.Id.ToString(), entity);
                return Ok(entity);
            }
        }

        public async Task<ServiceResponse> GetById(Guid id)
        {
            var entity = await _repository.GetById<SanPham>(id.ToString());
            if (entity == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);
            return Ok(entity);
        }

        public async Task<ServiceResponse> Delete(Guid id)
        {
            var entity = await _repository.GetById<SanPham>(id.ToString());
            if (entity == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);
            await _repository.DeleteAsync<SanPham>(id.ToString(), entity);
            return Ok(entity);
        }

        public async Task<ServiceResponse> GetAll(ProductRequest request)
        {
            var result = new PaginationResult<SanPham>();
            if (request.PageIndex <= 0)
            {
                request.PageIndex = 1;
            }
            if (request.PageSize <= 0)
            {
                request.PageSize = 10;
            }

            var query = new BsonDocument();

            if (!string.IsNullOrWhiteSpace(request.TenSanPham))
            {
                query.Add(nameof(SanPham.TenSanPhamKd), BsonRegularExpression.Create(new Regex(request.TenSanPham.ConvertToUnSign())));
            }

            if (!string.IsNullOrWhiteSpace(request.TenDanhMuc))
            {
                query.Add(nameof(SanPham.TenDanhMucKd), BsonRegularExpression.Create(new Regex(request.TenDanhMuc.ConvertToUnSign())));
            }

            if (!string.IsNullOrWhiteSpace(request.TenCuaHang))
            {
                query.Add(nameof(SanPham.TenCuaHangKd), BsonRegularExpression.Create(new Regex(request.TenCuaHang.ConvertToUnSign())));
            }

            if (!string.IsNullOrWhiteSpace(request.MoTa))
            {
                query.Add(nameof(SanPham.MoTakd), BsonRegularExpression.Create(new Regex(request.MoTa.ConvertToUnSign())));
            }

            if (request.SoLuong != null)
            {
                query.Add(nameof(SanPham.SoLuong), request.SoLuong);
            }

            if (request.DonGia != null)
            {
                query.Add(nameof(SanPham.DonGia), request.DonGia);
            }
            var data = _repository.FindForPageAsync<SanPham>(query, request.PageIndex, request.PageSize);
            var count = _repository.CountAsync<SanPham>(query);
            await Task.WhenAll(data, count);

            return Ok(result.Page(data.Result, request.PageIndex, request.PageSize, count.Result));
        }
    }
}
