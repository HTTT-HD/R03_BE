using Common.Helpers;
using Common.ViewModels.Store;

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

using System;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class CuaHang : BaseModel
    {
        public string TenCuaHang { get; set; }
        [JsonIgnore]
        public string TenCuaHangKd { get { return TenCuaHang.ConvertToUnSign(); } set { } }
       
        public string MoTa { get; set; }
        [JsonIgnore]
        public string MoTaKd { get { return MoTa.ConvertToUnSign(); } set { } }

        public string DiaChi { get; set; }
        [JsonIgnore]
        public string DiaChiKd { get { return DiaChi.ConvertToUnSign(); } set { } }

        public string SoDienThoai { get; set; }
        public string AnhDaiDien { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid ThanhVienId { get; set; }

        public string TenThanhVien { get; set; }
        [JsonIgnore]
        public string TenThanhVienKd { get { return TenThanhVien.ConvertToUnSign(); } set { } }
    }
}
