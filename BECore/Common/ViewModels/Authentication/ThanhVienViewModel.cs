using Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common.ViewModels.Authentication
{
    public class IdentityBase
    {
        public string MaThanhVien { get; set; }
        public string TenThanhVien { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
        public enumGioiTinh? GioiTinh { get; set; }
        public string CMND { get; set; }
    }

    public class IdentityViewModel : IdentityBase
    {
        [Required, MaxLength(60)]
        public string TenDangNhap { get; set; }
        [Required, MaxLength(120)]
        public string MatKhau { get; set; }
    }

    public class IdentityRequest : IdentityBase, IPageFilter
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class IdentityUpdate : IdentityBase
    {
        public Guid Id { get; set; }
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
}
