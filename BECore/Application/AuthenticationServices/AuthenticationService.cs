using AutoMapper;
using Common.Helpers;
using Common.Utils;
using Common.ViewModels.Authentication;
using Domain;
using Domain.Models;
using Domain.Repositories;
using ElasticSearch.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.AuthenticationServices
{
    public class AuthenticationService : BasePrincipal, IAuthenticationService
    {
        private readonly IMongoRepository _mongoRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IES_ThanhVien _es;

        public AuthenticationService(
            IHttpContextAccessor httpContextAccessor,
            IMongoRepository mongoRepository,
            IMapper mapper,
            IConfiguration configuration, 
            IES_ThanhVien es) : base(httpContextAccessor)
        {
            _mongoRepository=mongoRepository;
            _mapper=mapper;
            _configuration = configuration;
            _es = es;
        }

        public async Task<ServiceResponse> CreateAsync(IdentityViewModel model)
        {
            var check = await _mongoRepository.AnyLiqnAsync<ThanhVien>(x => x.TenDangNhap == model.TenDangNhap || x.MaThanhVien == model.MaThanhVien);
            if (check) return Conflict(Constants.CodeError.Conflict, Constants.MessageResponse.ConflictUser);

            var thanhVien = _mapper.Map<ThanhVien>(model);
            thanhVien.MatKhau = model.MatKhau.HashSha512();
            await _mongoRepository.AddAsync<ThanhVien>(thanhVien);
            return Created(thanhVien);
        }

        public async Task<ServiceResponse> UpdateAsync(IdentityUpdate model)
        {
            var check = await _mongoRepository.AnyLiqnAsync<ThanhVien>(x => x.Id != model.Id && x.MaThanhVien == model.MaThanhVien);
            if (check) return Conflict(Constants.CodeError.Conflict, Constants.MessageResponse.ConflictUser);

            var thanhVien = await _mongoRepository.GetById<ThanhVien>(model.Id.ToString());
            if (thanhVien == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);

            thanhVien.MaThanhVien = model.MaThanhVien;
            thanhVien.TenThanhVien = model.TenThanhVien;
            thanhVien.SoDienThoai = model.SoDienThoai;
            thanhVien.DiaChi = model.DiaChi;
            thanhVien.CMND = model.CMND;
            thanhVien.GioiTinh = model.GioiTinh.GetValueOrDefault();
            thanhVien.Img = model.Img;

            await _mongoRepository.UpdateAsnyc<ThanhVien>(thanhVien.Id.ToString(), thanhVien);
            return Ok(thanhVien);
        }

        public async Task<ServiceResponse> GetAll(IdentityRequest request)
        {
            if (request.PageIndex <= 0)
            {
                request.PageIndex = 1;
            }
            if (request.PageSize <= 0)
            {
                request.PageSize = int.MaxValue;
            }

            var result = new PaginationResult<ThanhVien>();
            var query = new QueryDocument();
            if (!string.IsNullOrWhiteSpace(request.MaThanhVien))
            {
                query.Add(nameof(ThanhVien.MaThanhVien), BsonRegularExpression.Create(new Regex(request.MaThanhVien)));
            }
            if (!string.IsNullOrWhiteSpace(request.TenThanhVien))
            {
                query.Add(nameof(ThanhVien.TenThanhVienKd), BsonRegularExpression.Create(new Regex(request.TenThanhVien.ConvertToUnSign())));
            }
            if (!string.IsNullOrWhiteSpace(request.SoDienThoai))
            {
                query.Add(nameof(ThanhVien.SoDienThoai), BsonRegularExpression.Create(new Regex(request.SoDienThoai)));
            }
            if (!string.IsNullOrWhiteSpace(request.DiaChi))
            {
                query.Add(nameof(ThanhVien.DiaChiKd), BsonRegularExpression.Create(new Regex(request.DiaChi.ConvertToUnSign())));
            }
            if (!string.IsNullOrWhiteSpace(request.CMND))
            {
                query.Add(nameof(ThanhVien.CMND), BsonRegularExpression.Create(new Regex(request.CMND)));
            }
            if (request.GioiTinh != null)
            {
                query.Add(nameof(ThanhVien.GioiTinh), request.GioiTinh);
            }
            var thanhViens = _mongoRepository.FindForPageAsync<ThanhVien>(query, request.PageIndex, request.PageSize);
            var count = _mongoRepository.CountAsync<ThanhVien>(query);
            await Task.WhenAll(thanhViens, count);
            return Ok(result.Page(thanhViens.Result, request.PageIndex, request.PageSize, count.Result));
        }

        public async Task<ServiceResponse> GetAllEs(IdentityRequest request)
        {
            return Ok(await _es.GetAll(request));
        }

        public async Task<ServiceResponse> GetById(Guid id)
        {
            var thanhVien = await _mongoRepository.GetById<ThanhVien>(id.ToString());
            if (thanhVien == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);

            return Ok(thanhVien);
        }

        public async Task<ServiceResponse> GetUserLogin()
        {
            var thanhVien = await _mongoRepository.GetById<ThanhVien>(_userId.ToString());
            if (thanhVien == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);

            return Ok(thanhVien);
        }

        public async Task<ServiceResponse> DeleteAsnyc(Guid id)
        {
            var thanhVien = await _mongoRepository.GetById<ThanhVien>(id.ToString());
            if (thanhVien == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);
            await _mongoRepository.DeleteAsync<ThanhVien>(id.ToString(), thanhVien);
            return Ok(true);
        }

        public async Task<ServiceResponse> LoginAction(LoginViewModel model)
        {
            var user = await _mongoRepository.FirsOfDefaultAsync<ThanhVien>(x => x.TenDangNhap == model.TaiKhoan);
            if (user == null) return BadRequest(Constants.CodeError.NotFound, Constants.MessageResponse.LoginFailed);
            if (!model.MatKhau.CheckHash(user.MatKhau))
            {
                return BadRequest(Constants.CodeError.NotFound, Constants.MessageResponse.LoginFailed);
            }

            var permissions = new List<Quyen>();
            var roles = await _mongoRepository.FindAllAsync<Vaitro>(x => x.ThanhViens != null && x.ThanhViens.Any(x => x == user.Id));
            if(roles != null && roles.Any())
            {
                var permissionIds = new List<Guid>();
                foreach(var role in roles)
                {
                    if(role.Quyen != null && role.Quyen.Any())
                    {
                        role.Quyen.ForEach(x => permissionIds.Add(x));
                    }
                }
                if(permissionIds != null && permissionIds.Any())
                {
                    permissions = await _mongoRepository.FindAllAsync<Quyen>(x => permissionIds.Contains(x.Id));
                }
            }
            var result = new LoginResponse()
            {
                HoTen = user.TenThanhVien,
                TenDangNhap = user.TenDangNhap,
                SoDienThoai = user.SoDienThoai,
                UserId = user.Id
            };
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, result.HoTen ?? String.Empty),
                new Claim(ClaimTypes.NameIdentifier, result.TenDangNhap ?? String.Empty),
                new Claim(Constants.Principal.SoDienThoai, result.SoDienThoai ?? String.Empty),
                new Claim(Constants.Principal.UserId, result.UserId.ToString())
            };
            if (permissions != null && permissions.Any())
            {
                result.Quyens = permissions.Select(x => x.Ma).ToList();
                var roleCode = string.Join(',', permissions.Select(x => x.Ma));
                authClaims.Add(new Claim(ClaimTypes.Role, roleCode ?? string.Empty));
            }
            var secret = _configuration.GetSection("JwtOptions:Secret")?.Value;
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtOptions:Issuer"],
                audience: _configuration["JwtOptions:Audience"],
                expires: DateTime.Now.AddMonths(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
             );

            result.AcessToken = new JwtSecurityTokenHandler().WriteToken(token);
            result.Expiration = token.ValidTo;

            return Ok(result);
        }

        public async Task<ServiceResponse> IndexUsers()
        {
            _es.DeleteIndex();
            var data = _mongoRepository.AsQueryable<ThanhVien>().Select(x => new UserESViewModel()
            {
                Id = x.Id,
                MaThanhVien = x.MaThanhVien,
                TenThanhVien = x.TenThanhVien,
                CMND = x.CMND,
                TenDangNhap = x.TenDangNhap,
                MatKhau = x.MatKhau,
                DiaChi = x.DiaChi,
                GioiTinh = x.GioiTinh,
                SoDienThoai = x.SoDienThoai,
                CreateAt = x.CreateAt
            });
            await _es.SynchronizedData(data);
            return Ok(true);
        }
    }
}
