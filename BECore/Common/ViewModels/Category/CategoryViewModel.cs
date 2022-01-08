using System;
using System.ComponentModel.DataAnnotations;

namespace Common.ViewModels.Category
{
    public class CategoryViewModel
    {
        public Guid? Id { get; set; }
        [Required, MaxLength(255)]
        public string TenDanhMuc { get; set; }
        [MaxLength(500)]
        public string MoTa { get; set; }
    }

    public class CategoryRequest : PageFilter
    {
        public string Ten { get; set; }
        public string MoTa { get; set; }

    }
}
