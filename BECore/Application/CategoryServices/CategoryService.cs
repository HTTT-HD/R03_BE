using AutoMapper;

using Common.Helpers;
using Common.Utils;
using Common.ViewModels.Category;
using Domain;
using Domain.Models;
using Domain.Repositories;
using Hangfire;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.CategoryServices
{
    public class CategoryService : BasePrincipal, ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IMongoRepository _repository;

        public CategoryService(IHttpContextAccessor httpContextAccessor, IMapper mapper, IMongoRepository repository) : base(httpContextAccessor)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResponse> AddOrUpdate(CategoryViewModel model)
        {
            if (model.Id == null || model.Id == Guid.Empty)
            {
                var entity = _mapper.Map<DanhMuc>(model); 
                await _repository.AddAsync<DanhMuc>(entity);
                return Created(entity);
            }
            else
            {
                var entity = await _repository.GetById<DanhMuc>(model.Id.ToString());
                if (entity == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);

                if (entity.TenDanhMuc != model.TenDanhMuc)
                {
                    BackgroundJob.Enqueue(() => UpdateProduct(model.TenDanhMuc, entity.Id));
                }
                entity = entity.DanhMuc(model);
                await _repository.UpdateAsnyc<DanhMuc>(entity.Id.ToString(), entity);
                return Ok(entity);
            }
        }

        public async Task<ServiceResponse> GetById(Guid id)
        {
            var entity = await _repository.GetById<DanhMuc>(id.ToString());
            if (entity == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);
            return Ok(entity);
        }

        public async Task<ServiceResponse> Delete(Guid id)
        {
            var entity = await _repository.GetById<DanhMuc>(id.ToString());
            if (entity == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);
            BackgroundJob.Enqueue(() => DeleteLogicProduc(id));
            await _repository.DeleteAsync<DanhMuc>(id.ToString(), entity);
            return Ok(entity);
        }

        public async Task<ServiceResponse> GetAll(CategoryRequest request)
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
            if (!string.IsNullOrWhiteSpace(request.Ten))
            {
                query.Add(nameof(DanhMuc.TenDanhMucKd), BsonRegularExpression.Create(new Regex(request.Ten.ConvertToUnSign())));
            }
            if (!string.IsNullOrWhiteSpace(request.MoTa))
            {
                query.Add(nameof(DanhMuc.MoTaKd), BsonRegularExpression.Create(new Regex(request.MoTa.ConvertToUnSign())));
            }

            var data = _repository.FindForPageAsync<DanhMuc>(query, request.PageIndex, request.PageSize);
            var count = _repository.CountAsync<DanhMuc>(query);
            await Task.WhenAll(data, count);
            var result = new PaginationResult<DanhMuc>();
            return Ok(result.Page(data.Result, request.PageIndex, request.PageSize, count.Result));
        }

        #region BackJob
        public async Task UpdateProduct(string tenDanhMuc, Guid danhMucId)
        {
            var sanPhams = await _repository.GetAllAsync<SanPham>(x => x.DanhMucId == danhMucId);
            if (sanPhams != null && sanPhams.Any())
            {
                foreach (var item in sanPhams)
                {
                    item.TenDanhMuc = tenDanhMuc;
                    await _repository.UpdateAsnyc<SanPham>(item.Id.ToString(), item);
                }
            }
        }

        public async Task DeleteLogicProduc(Guid danhMucId)
        {
            var sanPhams = await _repository.GetAllAsync<SanPham>(x => x.DanhMucId == danhMucId);
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
