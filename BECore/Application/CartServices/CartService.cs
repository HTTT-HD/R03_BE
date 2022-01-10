using Common.Helpers;
using Common.Utils;
using Common.ViewModels.Cart;
using Domain;
using Domain.Models;
using Domain.Repositories;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.CartServices
{
    public class CartService : BasePrincipal, ICartService
    {
        private readonly IMongoRepository _repository;

        public CartService(IHttpContextAccessor httpContextAccessor, IMongoRepository repository) : base(httpContextAccessor)
        {
            _repository = repository;
        }

        public async Task<ServiceResponse> AddProductToCart(AddProductToCart model)
        {
            var sanPham = await _repository.GetById<SanPham>(model.SanPhamId.ToString());
            if(sanPham == null || sanPham.CuaHangId != model.CuaHangId)
                return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);

            if(model.SoLuong > sanPham.SoLuong) return Forbidden(Constants.CodeError.NotEnough, Constants.MessageResponse.NotEnough);

            var bson = new BsonDocument();
            bson.Add(nameof(GioHang.CreateBy), _userId.ToString());
            bson.Add(nameof(GioHang.CuaHangId), model.CuaHangId.ToString());
            var gioHang = await _repository.FirsOfDefaultAsync<GioHang>(bson);
            var thanhTien = model.SoLuong * sanPham.DonGia;
            if(gioHang == null)
            {
                gioHang = new GioHang()
                {
                    CuaHangId = model.CuaHangId,
                    TongTien = thanhTien,
                    TongSoLuong = model.SoLuong,
                    ChiTiets = new List<ChiTietGioHang>()
                    {
                        new ChiTietGioHang()
                        {
                            SanPhamId = model.SanPhamId,
                            SoLuong = model.SoLuong,
                            ThanhTien = thanhTien
                        }
                    }
                };
                await _repository.AddAsync<GioHang>(gioHang);
                return Ok(true);
            }
            else
            {
                gioHang.TongTien += thanhTien;
                gioHang.TongSoLuong += model.SoLuong;

                ChiTietGioHang chiTiet = null;
                if(gioHang.ChiTiets != null && gioHang.ChiTiets.Any())
                {
                    chiTiet = gioHang.ChiTiets.Find(x => x.SanPhamId == model.SanPhamId);
                }

                if(chiTiet == null)
                {
                    chiTiet = new ChiTietGioHang()
                    {
                        SanPhamId = model.SanPhamId,
                        SoLuong = model.SoLuong,
                        ThanhTien = thanhTien
                    };
                    
                    gioHang.ChiTiets.Add(chiTiet);
                    await _repository.UpdateAsnyc<GioHang>(gioHang.Id.ToString(), gioHang);
                    return Ok(true);
                }
                else
                {
                    if(sanPham.SoLuong < model.SoLuong + chiTiet.SoLuong)
                        return Forbidden(Constants.CodeError.NotEnough, Constants.MessageResponse.NotEnough);

                    chiTiet.SoLuong += model.SoLuong;
                    chiTiet.ThanhTien += thanhTien;
                    await _repository.UpdateAsnyc<GioHang>(gioHang.Id.ToString(), gioHang);
                    return Ok(true);
                }
            }

            
        }

        public async Task<ServiceResponse> RemoveGioHang(Guid cuaHangId)
        {
            var bson = new BsonDocument();
            bson.Add(nameof(GioHang.CreateBy), _userId.ToString());
            bson.Add(nameof(GioHang.CuaHangId), cuaHangId.ToString());
            var gioHang = await _repository.FirsOfDefaultAsync<GioHang>(bson);
            if(gioHang != null)
            {
                await _repository.Remove<GioHang>(gioHang.Id.ToString());
            }
            return Ok(true);
        }

        public async Task<ServiceResponse> RemoveProduct(Guid cuaHangId, Guid sanPhamId)
        {
            var sanPham = await _repository.GetById<SanPham>(sanPhamId.ToString());
            if (sanPham == null || sanPham.CuaHangId != cuaHangId)
                return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);

            var bson = new BsonDocument();
            bson.Add(nameof(GioHang.CreateBy), _userId.ToString());
            bson.Add(nameof(GioHang.CuaHangId), cuaHangId.ToString());
            var gioHang = await _repository.FirsOfDefaultAsync<GioHang>(bson);
            if (gioHang != null)
            {
                if(gioHang.ChiTiets == null || !gioHang.ChiTiets.Any())
                    return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);

                var chiTiet = gioHang.ChiTiets.Find(x => x.SanPhamId == sanPhamId);
                if(chiTiet == null) 
                    return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);

                gioHang.TongTien -= chiTiet.ThanhTien;
                gioHang.TongSoLuong -= chiTiet.SoLuong;
                gioHang.ChiTiets.Remove(chiTiet);
                await _repository.UpdateAsnyc<GioHang>(gioHang.Id.ToString(), gioHang);
            }
            return Ok(gioHang);
        }

        public async Task<ServiceResponse> ChangeQuantity(AddProductToCart model)
        {
            var sanPham = await _repository.GetById<SanPham>(model.SanPhamId.ToString());
            if (sanPham == null || sanPham.CuaHangId != model.CuaHangId)
                return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);

            if (model.SoLuong > sanPham.SoLuong) return Forbidden(Constants.CodeError.NotEnough, Constants.MessageResponse.NotEnough);

            var bson = new BsonDocument();
            bson.Add(nameof(GioHang.CreateBy), _userId.ToString());
            bson.Add(nameof(GioHang.CuaHangId), model.CuaHangId.ToString());
            var gioHang = await _repository.FirsOfDefaultAsync<GioHang>(bson);

            if (gioHang == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);
            if (gioHang.ChiTiets == null || !gioHang.ChiTiets.Any())
                return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);

            var chiTiet = gioHang.ChiTiets.Find(x => x.SanPhamId == model.SanPhamId);
            if(chiTiet == null) return NotFound(Constants.CodeError.NotFound, Constants.MessageResponse.NotFound);

            var soLuong = model.SoLuong - chiTiet.SoLuong;
            var tongTien = soLuong * sanPham.DonGia;
            chiTiet.ThanhTien = model.SoLuong * sanPham.DonGia;
            chiTiet.SoLuong = model.SoLuong;
            gioHang.TongTien += tongTien;
            gioHang.TongSoLuong += soLuong;

            await _repository.UpdateAsnyc<GioHang>(gioHang.Id.ToString(), gioHang);
            var result = new ChangeQuantityProductResponse()
            {
                ThanhTien = chiTiet.ThanhTien,
                TongSoLuong = gioHang.TongSoLuong,
                TongTien = gioHang.TongTien
            };
            return Ok(result);
        }

        public async Task<ServiceResponse> GetAll(Guid cuaHangId)
        {
            var bson = new BsonDocument();
            bson.Add(nameof(GioHang.CreateBy), _userId.ToString());
            bson.Add(nameof(GioHang.CuaHangId), cuaHangId.ToString());
            var gioHang = await _repository.FirsOfDefaultAsync<GioHang>(bson);
            var result = new CartViewModel()
            {
                CuaHangId = cuaHangId,
                TongSoLuong = 0,
                TongTien = 0,
                Products = new List<ProductsInCart>()
            };
            if (gioHang == null || gioHang.ChiTiets == null || !gioHang.ChiTiets.Any()) return Ok(result);

            var sanPhamIds = gioHang.ChiTiets.Select(x => x.SanPhamId.ToString()).ToList();
            var bson2 = new BsonDocument();
            bson2.Add("_id", new BsonDocument("$in", new BsonArray(sanPhamIds)));
            var sanPhams = await _repository.FindAllAsync<SanPham>(bson2);
            if(sanPhams != null && !sanPhams.Any()) return Ok(result);

            result.TongTien = gioHang.TongTien;
            result.TongSoLuong = gioHang.TongSoLuong;

            result.Products = from ct in gioHang.ChiTiets
                         join sp in sanPhams on ct.SanPhamId equals sp.Id
                         select new ProductsInCart()
                         {
                             SanPhamId = ct.SanPhamId,
                             SoLuong = ct.SoLuong,
                             TenSanPham = sp?.TenSanPham,
                             ThanhTien = ct.ThanhTien
                         };

            return Ok(result);
        }
    }
}
