using Common.Enums;
using Common.Helpers;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common.ViewModels.Order
{
    public class OrderViewModel
    {
        [Required, MaxLength(255)]
        public string NguoiDat { get; set; }
        [Required, MaxLength(12), Phone]
        public string SoDienThoai { get; set; }
        [MaxLength(500), Required]
        public string DiaChiNhan { get; set; }
        [Required]
        public Guid CuaHangId { get; set; }
        public enumPayment? LoaiThanhToan { get; set; }
    }

    public class UpdatePayment
    {
        public Guid DonHangId { get; set; }
        public enumPayment LoaiThanhToan { get; set; }
    }

    public class OrderRequest : PageFilter
    {
        public string NguoiDat { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChiNhan { get; set; }
        public string TenCuaHang { get; set; }
        public Guid? CuaHangId { get; set; }
        public enumStatus? TrangThai { get; set; }
        public enumPayment? LoaiThanhToan { get; set; }
        public int? TongSoLuong { get; set; }
        public long? TongTien { get; set; }
        public string TuNgay { get; set; }
        public string DenNgay { get; set; }
    }

    public class OrderDetail
    {
        public Guid Id { get; set; }

        public Guid ThanhVienId { get; set; }
        public string NguoiDat { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChiNhan { get; set; }
        public Guid? NguoiGiaoId { get; set; }
        public string TenNguoiGiao { get; set; }
        public Guid? CuaHangId { get; set; }
        public string TenCuaHang { get; set; }
        public enumStatus TrangThai { get; set; }
        public string TenTrangThai { get; set; }

        public enumPayment LoaiThanhToan { get; set; }
        public string TenLoaiThanhToan { get; set; }

        public int TongSoLuong { get; set; }
        public long TongTien { get; set; }
        public long TienShip { get; set; }
        public DateTime NgayTao { get; set; }
        public List<ChiTiet> ChiTiets { get; set; }
    }
    public class ChiTiet
    {
        public Guid SanPhamId { get; set; }
        public string TenSanPham { get; set; }
        public string Img { get; set; }
        public int SoLuong { get; set; }
        public long DonGia { get; set; }
        public long ThanhTien { get; set; }
    }

    public class DashboardResponse
    {
        public long TienTuCuaHang { get; set; }
        public long TienTuShip { get; set; }
    }
}
