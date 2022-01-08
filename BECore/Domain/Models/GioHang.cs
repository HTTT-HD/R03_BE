using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class GioHang : BaseModel
    {
        [BsonRepresentation(BsonType.String)]
        public Guid CuaHangId { get; set; }
        public long TongTien { get; set; }
        public int TongSoLuong { get; set; }
        [JsonIgnore]
        public List<ChiTietGioHang> ChiTiets { get; set; }
    }

    public class ChiTietGioHang
    {
        [BsonRepresentation(BsonType.String)]
        public Guid SanPhamId { get; set; }
        public int SoLuong { get; set; }
        public long ThanhTien { get; set; }
    }
}
