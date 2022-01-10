using AutoMapper;

using Common.Enums;
using Common.Helpers;
using Common.Utils;
using Common.ViewModels.Cart;
using Common.ViewModels.Order;

using Domain;
using Domain.Models;
using Domain.Repositories;

using Hangfire;

using Microsoft.AspNetCore.Http;

using MongoDB.Bson;
using MongoDB.Driver.Builders;

using Nest;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.OrderServices
{
    public class OrderService : BasePrincipal, IOrderService
    {
        private readonly IMongoRepository _repository;
        private readonly IMapper _mapper;

        public OrderService(IHttpContextAccessor httpContextAccessor, IMongoRepository repository, IMapper mapper) : base(httpContextAccessor)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> Order(OrderViewModel model)
        {
            var cuaHang = await _repository.GetById<CuaHang>(model.CuaHangId.ToString());
            if (cuaHang == null)
                return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);

            var bson = new BsonDocument();
            bson.Add(nameof(GioHang.CreateBy), _userId.ToString());
            bson.Add(nameof(GioHang.CuaHangId), model.CuaHangId.ToString());
            var gioHang = await _repository.FirsOfDefaultAsync<GioHang>(bson);

            if (gioHang == null || gioHang.ChiTiets == null || !gioHang.ChiTiets.Any())
                return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);

            var sanPhamIds = gioHang.ChiTiets.Select(x => x.SanPhamId.ToString()).ToList();
            bson = new BsonDocument();
            bson.Add("_id", new BsonDocument("$in", new BsonArray(sanPhamIds)));
            var sanPhams = await _repository.FindAllAsync<SanPham>(bson);
            if (sanPhams != null && !sanPhams.Any())
                return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);

            var chiTiets = new List<ChiTietDonHang>();
            foreach (var item in gioHang.ChiTiets)
            {
                var sanPham = await _repository.GetById<SanPham>(item.SanPhamId.ToString());
                if (sanPham == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);
                if (sanPham.SoLuong < item.SoLuong) return NotFound(Constants.CodeError.NotEnough, Constants.MessageResponse.NotEnough);

                chiTiets.Add(new ChiTietDonHang()
                {
                    SanPhamId = item.SanPhamId,
                    SoLuong = item.SoLuong,
                    TenSanPham = sanPham.TenSanPham,
                    ThanhTien = item.ThanhTien,
                    Img = sanPham.Img,
                    DonGia = sanPham.DonGia
                });
                sanPham.SoLuong -= item.SoLuong;
                await _repository.UpdateAsnyc<SanPham>(sanPham.Id.ToString(), sanPham);
            }
            var hoaDon = new DonHang()
            {
                ThanhVienId = _userId,
                NguoiDat = model.NguoiDat,
                DiaChiNhan = model.DiaChiNhan,
                SoDienThoai = model.SoDienThoai,
                CuaHangId = cuaHang.Id,
                TenCuaHang = cuaHang.TenCuaHang,
                TongSoLuong = gioHang.TongSoLuong,
                TongTien = gioHang.TongTien,
                TrangThai = enumStatus.NhanDon,
                ChiTiets = chiTiets,
                NgayTao = DateTime.Now,
                TienShip = 15000
            };
            await _repository.AddAsync<DonHang>(hoaDon);
            return Ok(true);
        }

        public async Task<ServiceResponse> RejectOrder(Guid donHangId)
        {
            var hoaDon = await _repository.GetById<DonHang>(donHangId.ToString());
            if (hoaDon == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);
            if (hoaDon.ChiTiets != null && hoaDon.ChiTiets.Any())
            {
                BackgroundJob.Enqueue(() => JobRejectOrder(hoaDon.ChiTiets));
            }
            hoaDon.TrangThai = enumStatus.HuyDon;
            await _repository.UpdateAsnyc<DonHang>(hoaDon.Id.ToString(), hoaDon);
            return Ok(true);
        }

        public async Task<ServiceResponse> TransportOrder(Guid donHangId)
        {
            var hoaDon = await _repository.GetById<DonHang>(donHangId.ToString());
            if (hoaDon == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);
            hoaDon.TrangThai = enumStatus.DangGiao;
            await _repository.UpdateAsnyc<DonHang>(hoaDon.Id.ToString(), hoaDon);
            return Ok(true);
        }

        public async Task<ServiceResponse> FinishOrder(Guid donHangId)
        {
            var hoaDon = await _repository.GetById<DonHang>(donHangId.ToString());
            if (hoaDon == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);
            hoaDon.TrangThai = enumStatus.DaGiao;
            await _repository.UpdateAsnyc<DonHang>(hoaDon.Id.ToString(), hoaDon);
            return Ok(true);
        }

        public async Task<ServiceResponse> GetAll(OrderRequest request)
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
            if (!string.IsNullOrWhiteSpace(request.TuNgay) && !string.IsNullOrWhiteSpace(request.DenNgay))
            {
                var fromDate = request.TuNgay.ConvertToDateTimeFormat();
                var toDate = request.DenNgay.ConvertToDateTimeFormat();
                query = new BsonDocument {
                   {
                      nameof(DonHang.NgayTao),
                      new BsonDocument {
                         {
                            "$gt",
                            fromDate
                         }, {
                            "$lt",
                            toDate
                         }
                      }
                   }
                };
            }

            if (request.IsUser)
            {
                query.Add(nameof(DonHang.ThanhVienId), _userId.ToString());
            }

            if (!string.IsNullOrWhiteSpace(request.TenCuaHang))
            {
                query.Add(nameof(DonHang.TenCuaHangKd), BsonRegularExpression.Create(new Regex(request.TenCuaHang.ConvertToUnSign())));
            }

            if (!string.IsNullOrWhiteSpace(request.NguoiDat))
            {
                query.Add(nameof(DonHang.NguoiDatKd), BsonRegularExpression.Create(new Regex(request.NguoiDat.ConvertToUnSign())));
            }

            if (!string.IsNullOrWhiteSpace(request.DiaChiNhan))
            {
                query.Add(nameof(DonHang.DiaChiNhanKd), BsonRegularExpression.Create(new Regex(request.DiaChiNhan.ConvertToUnSign())));
            }

            if (!string.IsNullOrWhiteSpace(request.SoDienThoai))
            {
                query.Add(nameof(DonHang.SoDienThoai), request.SoDienThoai);
            }

            if (request.TrangThai != null)
            {
                query.Add(nameof(DonHang.TrangThai), request.TrangThai);
            }

            if (request.TongSoLuong != null)
            {
                query.Add(nameof(DonHang.TongSoLuong), request.TongSoLuong);
            }

            var data = _repository.FindForPageAsync<DonHang>(query, request.PageIndex, request.PageSize);
            var count = _repository.CountAsync<DonHang>(query);
            await Task.WhenAll(data, count);
            var result = new PaginationResult<DonHang>();
            return Ok(result.Page(data.Result, request.PageIndex, request.PageSize, count.Result));
        }

        public async Task<ServiceResponse> OrderDetail(Guid donHangId)
        {
            var hoaDon = await _repository.GetById<DonHang>(donHangId.ToString());
            if (hoaDon == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);
            var result = _mapper.Map<OrderDetail>(hoaDon);
            return Ok(result);
        }

        public async Task<ServiceResponse> Dashboard(string tuNgay, string denNgay)
        {
            var query = new BsonDocument();
            if (!string.IsNullOrWhiteSpace(tuNgay) && !string.IsNullOrWhiteSpace(tuNgay))
            {
                var fromDate = tuNgay.ConvertToDateTimeFormat();
                var toDate = denNgay.ConvertToDateTimeFormat();
                query = new BsonDocument {
                   {
                      nameof(DonHang.NgayTao),
                      new BsonDocument {
                         {
                            "$gt",
                            fromDate
                         }, {
                            "$lt",
                            toDate
                         }
                      }
                   }
                };
            }
            var data = await _repository.FindAllAsync<DonHang>(query);
            var result = new DashboardResponse()
            {
                TienTuCuaHang = 0,
                TienTuShip = 0
            };
            if(data != null)
            {
                result.TienTuCuaHang = data.Sum(x => x.TongTien) * Constants.AuthConfig.Store / 100;
                result.TienTuShip = data.Sum(x => x.TienShip) * Constants.AuthConfig.Shiper / 100;
            }
            return Ok(result);
        }

        #region Background job
        public async Task JobRejectOrder(List<ChiTietDonHang> chiTiets)
        {
            foreach (var item in chiTiets)
            {
                var sanPham = await _repository.GetById<SanPham>(item.SanPhamId.ToString());
                if (sanPham != null)
                {
                    sanPham.SoLuong += item.SoLuong;
                    await _repository.UpdateAsnyc<SanPham>(sanPham.Id.ToString(), sanPham);
                }
            }
        }
        #endregion
    }
}
