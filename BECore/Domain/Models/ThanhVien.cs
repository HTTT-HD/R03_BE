using Common.Enums;
using Common.Helpers;

using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class ThanhVien : BaseModel
    {
        public string MaThanhVien { get; set; }
        public string TenThanhVien { get; set; }
        [JsonIgnore]
        public string TenThanhVienKd { get { return TenThanhVien.ConvertToUnSign(); } set { } }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
        [JsonIgnore]
        public string DiaChiKd { get { return DiaChi.ConvertToUnSign(); } set { } }
        public enumGioiTinh GioiTinh { get; set; }
        public string TenGioiTinh { get { return GioiTinh.GetDisplayName(); } set { } }
        public string CMND { get; set; }
        public string TenDangNhap { get; set; }
        [JsonIgnore]
        public string MatKhau { get; set; }
    }
}
