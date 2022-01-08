using Common.Enums;
using Common.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Common.ViewModels.Authentication
{
    public class IdentityRequest : PageFilter
    {
        public string MaThanhVien { get; set; }
        public string TenThanhVien { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
        public enumGioiTinh? GioiTinh { get; set; }
        public string CMND { get; set; }
    }

    public class IdentityBaseRequried
    {
        [Required, MaxLength(20)]
        public string MaThanhVien { get; set; }
        [Required, MaxLength(255)]
        public string TenThanhVien { get; set; }
        [Required, MaxLength(12), Phone]
        public string SoDienThoai { get; set; }
        [MaxLength(500)]
        public string DiaChi { get; set; }
        public enumGioiTinh? GioiTinh { get; set; }
        [MaxLength(12)]
        public string CMND { get; set; }
    }

    public class IdentityViewModel : IdentityBaseRequried
    {
        [Required, MaxLength(60)]
        public string TenDangNhap { get; set; }
        [Required, MaxLength(120)]
        public string MatKhau { get; set; }
        public string Img { get; set; }
    }

    public class IdentityUpdate : IdentityBaseRequried
    {
        public Guid Id { get; set; }
        public string Img { get; set; }
    }

    public class LoginViewModel
    {
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
    }

    public class LoginResponse
    {
        public string AcessToken { get; set; }
        public DateTime Expiration { get; set; }
        public List<string> Quyens { get; set; }
        public string HoTen { get; set; }
        public string TenDangNhap { get; set; }
        public string SoDienThoai { get; set; }
        public Guid UserId { get; set; }
    }

    public class UserInRole : IdentityRequest
    {
        public Guid VaiTroId { get; set; }
    }

    public class UserESViewModel : IdentityBaseRequried
    {
        public Guid Id { get; set; }
        public string TenDangNhap { get; set; }
        [JsonIgnore]
        public string MatKhau { get; set; }
        public string TenGioiTinh { get { return GioiTinh.GetDisplayName(); } set { } }
        [JsonIgnore]
        public DateTime CreateAt { get; set; }
    }
}
