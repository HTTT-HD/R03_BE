using Common.Helpers;
using Common.Utils;
using Common.ViewModels.Authentication;
using Common.ViewModels.Permission;
using Common.ViewModels.Role;

using Domain;
using Domain.Models;
using Domain.Repositories;

using Microsoft.AspNetCore.Http;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.RoleServices
{
    public class RoleService : BasePrincipal, IRoleService
    {
        private readonly IMongoRepository _repository;

        public RoleService(IHttpContextAccessor httpContextAccessor, IMongoRepository repository) : base(httpContextAccessor)
        {
            _repository=repository;
        }

        public async Task<ServiceResponse> AddOrUpdate(RoleViewModel model)
        {
            if (model.Id == null || model.Id == Guid.Empty)
            {
                var entity = new Vaitro()
                {
                    TenVaiTro = model.TenVaiTro,
                    Quyen = new List<Guid>(),
                    ThanhViens = new List<Guid>(),
                };

                await _repository.AddAsync<Vaitro>(entity);
                return Created(entity);
            }
            else
            {
                var entity = await _repository.GetById<Vaitro>(model.Id.ToString());
                if (entity == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);

                entity.TenVaiTro = model.TenVaiTro;

                await _repository.UpdateAsnyc<Vaitro>(entity.Id.ToString(), entity);
                return Ok(entity);
            }
        }

        public async Task<ServiceResponse> GetById(Guid id)
        {
            var entity = await _repository.GetById<Vaitro>(id.ToString());
            if (entity == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);
            return Ok(entity);
        }
        public async Task<ServiceResponse> DeleteAsnyc(Guid id)
        {
            var entity = await _repository.GetById<Vaitro>(id.ToString());
            if (entity == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);
            await _repository.DeleteAsync(entity.Id.ToString(), entity);
            return Ok(true);
        }

        public async Task<ServiceResponse> GetAll(RoleRequest request)
        {
            if (request.PageIndex <= 0)
            {
                request.PageIndex = 1;
            }
            if (request.PageSize <= 0)
            {
                request.PageSize = int.MaxValue;
            }
            var result = new PaginationResult<Vaitro>();
            var query = new QueryDocument();
            if (!string.IsNullOrWhiteSpace(request.TenVaiTro))
            {
                query.Add(nameof(Vaitro.TenKd), BsonRegularExpression.Create(new Regex(request.TenVaiTro)));
            }
            var data = _repository.FindForPageAsync<Vaitro>(query, request.PageIndex, request.PageSize);
            var count = _repository.CountAsync<Vaitro>(query);
            await Task.WhenAll(data, count);
            return Ok(result.Page(data.Result, request.PageIndex, request.PageSize, count.Result));
        }

        public async Task<ServiceResponse> AddPermission(AddPermisisonToRole request)
        {
            var vaiTro = await _repository.GetById<Vaitro>(request.VaiTroId.ToString());
            if (vaiTro == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);

            var permission = await _repository.GetById<Quyen>(request.QuyenId.ToString());
            if (permission == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);

            if (vaiTro.Quyen == null && !vaiTro.Quyen.Any())
            {
                vaiTro.Quyen = new List<Guid>();
            }

            if (request.Them)
            {
                var check = vaiTro.Quyen.Any(x => x == request.QuyenId);
                if (check) return Conflict(Constants.CodeError.Conflict, Constants.MessageResponse.ConflictPermission);

                vaiTro.Quyen.Add(request.QuyenId);
            }
            else
            {
                var check = vaiTro.Quyen.Any(x => x == request.QuyenId);
                if (!check) return BadRequest(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);

                vaiTro.Quyen.Remove(request.QuyenId);
            }

            await _repository.UpdateAsnyc<Vaitro>(vaiTro.Id.ToString(), vaiTro);
            return Ok(true);
        }

        public async Task<ServiceResponse> AddUser(AddUserToRole request)
        {
            var vaiTro = await _repository.GetById<Vaitro>(request.VaiTroId.ToString());
            if (vaiTro == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);

            var thanhVien = await _repository.GetById<ThanhVien>(request.ThanhVienId.ToString());
            if (thanhVien == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);

            if (vaiTro.ThanhViens == null && !vaiTro.ThanhViens.Any())
            {
                vaiTro.ThanhViens = new List<Guid>();
            }

            if (request.Them)
            {
                var check = vaiTro.ThanhViens.Any(x => x == request.ThanhVienId);
                if (check) return Conflict(Constants.CodeError.Conflict, Constants.MessageResponse.ConflictUserInRole);

                vaiTro.ThanhViens.Add(request.ThanhVienId);
            }
            else
            {
                var check = vaiTro.ThanhViens.Any(x => x == request.ThanhVienId);
                if (!check) return BadRequest(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);

                vaiTro.ThanhViens.Remove(request.ThanhVienId);
            }

            await _repository.UpdateAsnyc<Vaitro>(vaiTro.Id.ToString(), vaiTro);
            return Ok(true);
        }

        public async Task<ServiceResponse> GetPermissionInRole(PermissionRequest request)
        {
            var entity = await _repository.GetById<Vaitro>(request.VaiTroId.ToString());
            if (entity == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);

            var query = new QueryDocument();
            if (!string.IsNullOrWhiteSpace(request.Ma))
            {
                query.Add(nameof(Quyen.Ma), BsonRegularExpression.Create(new Regex(request.Ma)));
            }
            if (!string.IsNullOrWhiteSpace(request.Ten))
            {
                query.Add(nameof(Quyen.Ten), BsonRegularExpression.Create(new Regex(request.Ten.ConvertToUnSign())));
            }
            var data = await _repository.FindAllAsync<Quyen>(query);
            if (data != null && data.Any())
            {
                var permissions = data.Select(x => new PermissionResponse()
                {
                    Id = x.Id,
                    Ma = x.Ma,
                    Ten = x.Ten,
                    SuDung = (entity.Quyen != null && entity.Quyen.Any()) ? entity.Quyen.Contains(x.Id) : false
                });
                return Ok(permissions);
            }
            return Ok();
        }

        public async Task<ServiceResponse> GetUserInRole(UserInRole request)
        {
            var entity = await _repository.GetById<Vaitro>(request.VaiTroId.ToString());
            if (entity == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);

            var thanhViens = _repository.AsQueryable<ThanhVien>();
            if (entity.ThanhViens != null && entity.ThanhViens.Any())
            {
                thanhViens = thanhViens.Where(x => entity.ThanhViens.Contains(x.Id));
            }
            if (!string.IsNullOrWhiteSpace(request.MaThanhVien))
            {
                request.MaThanhVien =  request.MaThanhVien.Trim().ToLower();
                thanhViens = thanhViens.Where(x => x.MaThanhVien.ToLower().Contains(request.MaThanhVien));
            }
            if (!string.IsNullOrWhiteSpace(request.TenThanhVien))
            {
                request.TenThanhVien =  request.TenThanhVien.ConvertToUnSign();
                thanhViens = thanhViens.Where(x => x.TenThanhVienKd.ToLower().Contains(request.TenThanhVien));
            }
            if (!string.IsNullOrWhiteSpace(request.SoDienThoai))
            {
                thanhViens = thanhViens.Where(x => x.SoDienThoai.Contains(request.SoDienThoai));
            }
            if (!string.IsNullOrWhiteSpace(request.DiaChi))
            {
                request.DiaChi =  request.DiaChi.ConvertToUnSign();
                thanhViens = thanhViens.Where(x => x.DiaChiKd.Contains(request.DiaChi));
            }
            if (!string.IsNullOrWhiteSpace(request.CMND))
            {
                request.CMND =  request.CMND.Trim();
                thanhViens = thanhViens.Where(x => x.CMND.Contains(request.CMND));
            }
            if (request.GioiTinh != null)
            {
                thanhViens = thanhViens.Where(x => x.GioiTinh == request.GioiTinh);
            }
            var result = new PaginationResult<ThanhVien>();

            var data = Task.Run(() =>
            {
                return thanhViens.OrderBy(x => x.CreateAt).Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize);
            });
            var count = Task.Run(() =>
            {
                return thanhViens.Count();
            });
            await Task.WhenAll(data, count);
            return Ok(result.Page(data.Result, request.PageIndex, request.PageSize, count.Result));
        }

        public async Task<ServiceResponse> GetUserOutRole(UserInRole request)
        {
            var entity = await _repository.GetById<Vaitro>(request.VaiTroId.ToString());
            if (entity == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);

            var thanhViens = _repository.AsQueryable<ThanhVien>();
            if (entity.ThanhViens != null && entity.ThanhViens.Any())
            {
                thanhViens = thanhViens.Where(x => !entity.ThanhViens.Contains(x.Id));
            }
            if (!string.IsNullOrWhiteSpace(request.MaThanhVien))
            {
                request.MaThanhVien =  request.MaThanhVien.Trim().ToLower();
                thanhViens = thanhViens.Where(x => x.MaThanhVien.ToLower().Contains(request.MaThanhVien));
            }
            if (!string.IsNullOrWhiteSpace(request.TenThanhVien))
            {
                request.TenThanhVien =  request.TenThanhVien.ConvertToUnSign();
                thanhViens = thanhViens.Where(x => x.TenThanhVienKd.ToLower().Contains(request.TenThanhVien));
            }
            if (!string.IsNullOrWhiteSpace(request.SoDienThoai))
            {
                thanhViens = thanhViens.Where(x => x.SoDienThoai.Contains(request.SoDienThoai));
            }
            if (!string.IsNullOrWhiteSpace(request.DiaChi))
            {
                request.DiaChi =  request.DiaChi.ConvertToUnSign();
                thanhViens = thanhViens.Where(x => x.DiaChiKd.Contains(request.DiaChi));
            }
            if (!string.IsNullOrWhiteSpace(request.CMND))
            {
                request.CMND =  request.CMND.Trim();
                thanhViens = thanhViens.Where(x => x.CMND.Contains(request.CMND));
            }
            if (request.GioiTinh != null)
            {
                thanhViens = thanhViens.Where(x => x.GioiTinh == request.GioiTinh);
            }
            var result = new PaginationResult<ThanhVien>();

            var data = Task.Run(() =>
            {
                return thanhViens.OrderBy(x => x.CreateAt).Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize);
            });
            var count = Task.Run(() =>
            {
                return thanhViens.Count();
            });
            await Task.WhenAll(data, count);
            return Ok(result.Page(data.Result, request.PageIndex, request.PageSize, count.Result));
        }
    }
}
