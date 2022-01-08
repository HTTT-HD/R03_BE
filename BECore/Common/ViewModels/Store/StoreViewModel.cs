using System;
using System.ComponentModel.DataAnnotations;

namespace Common.ViewModels.Store
{
    public class StoreBase : PageFilter
    {
        public string TenCuaHang { get; set; }
        public string MoTa { get; set; }
        public Guid ThanhVienId { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
    }

    public class StoreViewModel
    {
        public Guid? Id { get; set; }
        [Required, MaxLength(250)]
        public string TenCuaHang { get; set; }
        [MaxLength(500)]
        public string MoTa { get; set; }
        [Required]
        public string AnhDaiDien { get; set; }
        public Guid ThanhVienId { get; set; }
        [Required]
        public string DiaChi { get; set; }
        [Required, Phone, MaxLength(12)]
        public string SoDienThoai { get; set; }
    }
    public class StoreRequest : StoreBase
    {
        public string TenThanhVien { get; set; }
        public bool IsUser { get; set; }
    }
}
