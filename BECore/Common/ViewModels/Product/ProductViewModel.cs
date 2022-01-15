using System; 
using System.ComponentModel.DataAnnotations; 

namespace Common.ViewModels.Product
{
    public class ProductViewModel
    {
        public Guid? Id { get; set; }
        [Required, MaxLength(300)]
        public string TenSanPham { get; set; }

        [Required, Range(1, double.MaxValue)]
        public int SoLuong { get; set; }
        [Required, Range(1, double.MaxValue)]
        public long DonGia { get; set; }

        [Required]
        public Guid DanhMucId { get; set; } 
        
        public string Img { get; set; }

        [Required, MaxLength(500)]
        public string MoTa { get; set; }

        public string ChiTiet { get; set; }

        [Required]
        public Guid CuaHangId { get; set; }
    }

    public class ProductRequest : PageFilter
    {
        public string TenSanPham { get; set; }
        public int? SoLuong { get; set; }
        public long? DonGia { get; set; }
        public string TenDanhMuc { get; set; }
        public string MoTa { get; set; }
        public string TenCuaHang { get; set; }
    }
}
