using Common.ViewModels.Category;
using Common.ViewModels.Product;
using Common.ViewModels.Store;

namespace Domain.Models
{
    public static class RePlaceObject
    {
        public static CuaHang CuaHang(this CuaHang cuaHang, StoreViewModel model)
        {
            cuaHang.TenCuaHang = model.TenCuaHang;
            cuaHang.MoTa = model.MoTa;
            cuaHang.ThanhVienId = model.ThanhVienId;
            cuaHang.AnhDaiDien = model.AnhDaiDien;
            cuaHang.DiaChi = model.DiaChi;
            cuaHang.SoDienThoai = model.SoDienThoai;

            return cuaHang;
        }

        public static DanhMuc DanhMuc(this DanhMuc cuaHang, CategoryViewModel model)
        {
            cuaHang.MoTa = model.MoTa;
            cuaHang.TenDanhMuc = model.TenDanhMuc;

            return cuaHang;
        }

        public static SanPham SanPham(this SanPham sanPham, ProductViewModel model)
        {
            sanPham.TenSanPham = model.TenSanPham;
            sanPham.SoLuong = model.SoLuong;
            sanPham.DonGia = model.DonGia;
            sanPham.DanhMucId = model.DanhMucId;
            sanPham.Img = model.Img;
            sanPham.MoTa = model.MoTa;
            sanPham.ChiTiet = model.ChiTiet;
            sanPham.CuaHangId = model.CuaHangId;
            return sanPham;

        }
    }
}
