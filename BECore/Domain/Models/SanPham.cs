using Common.Helpers;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

using System;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class SanPham : BaseModel
    {
        public string TenSanPham { get; set; }
        [JsonIgnore]
        public string TenSanPhamKd { get { return TenSanPham.ConvertToUnSign(); } set { } }

        public int SoLuong { get; set; }
        public long DonGia { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid? DanhMucId { get; set; }

        public string TenDanhMuc { get; set; }
        [JsonIgnore]
        public string TenDanhMucKd { get { return TenDanhMuc.ConvertToUnSign(); } set { } }
        
        public string Img { get; set; } 
        public string MoTa { get; set; }
        [JsonIgnore]
        public string MoTakd { get { return TenDanhMuc.ConvertToUnSign(); } set { } }

        public string ChiTiet { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid? CuaHangId { get; set; }
        public string TenCuaHang { get; set; }
        [JsonIgnore]
        public string TenCuaHangKd { get { return TenCuaHang.ConvertToUnSign(); } set { } }
    }
}
