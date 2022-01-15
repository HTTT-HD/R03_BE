using Common.Enums;
using Common.Helpers;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization; 

namespace Domain.Models
{
    public class DonHang : BaseModel
    {
        [BsonRepresentation(BsonType.String)]
        public Guid ThanhVienId { get; set; }
        public string NguoiDat { get; set; }
        [JsonIgnore]
        public string NguoiDatKd { get { return NguoiDat.ConvertToUnSign(); } set { } }
        public string SoDienThoai { get; set; }
        public string DiaChiNhan { get; set; }
        [JsonIgnore]
        public string DiaChiNhanKd { get { return DiaChiNhan.ConvertToUnSign(); } set { } }

        [BsonRepresentation(BsonType.String)]
        public Guid? NguoiGiaoId { get; set; }
        public string TenNguoiGiao { get; set; }
        [JsonIgnore]
        public string TenNguoiGiaoKd { get { return TenNguoiGiao.ConvertToUnSign(); } set { } }

        [BsonRepresentation(BsonType.String)]
        public Guid? CuaHangId { get; set; }
        public string TenCuaHang { get; set; }
        public string TenCuaHangKd { get { return TenCuaHang.ConvertToUnSign(); } set { } }

        public enumStatus TrangThai { get; set; }
        
        public string TenTrangThai { get { return TrangThai.GetDisplayName(); } set { } }

        public enumPayment? LoaiThanhToan { get; set; }
        public string TenLoaiThanhToan { get { return LoaiThanhToan.GetDisplayName(); } set { } }

        public int TongSoLuong { get; set; }
        public long TongTien { get; set; }
        public long TienShip { get; set; }
        
        public DateTime NgayTao { get; set; }
        
        public List<ChiTietDonHang> ChiTiets { get; set; }
    }
    public class ChiTietDonHang
    {
        [BsonRepresentation(BsonType.String)]
        public Guid SanPhamId { get; set; }
        public string TenSanPham { get; set; }
        public string Img { get; set; }
        public int SoLuong { get; set; }
        public long DonGia { get; set; }
        public long ThanhTien { get; set; }
    }
}
