using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common.ViewModels.Cart
{
    public class AddProductToCart
    {
        public Guid CuaHangId { get; set; }
        public Guid SanPhamId { get; set; }
        [Required, Range(1, int.MaxValue)]
        public int SoLuong { get; set; }
    }

    public class ChangeQuantityProductResponse
    {
        public long TongTien { get; set; }
        public int TongSoLuong { get; set; }
        public long ThanhTien { get; set; }
    }

    public class CartViewModel
    {
        public Guid CuaHangId { get; set; }
        public int TongSoLuong { get; set; }
        public long TongTien { get; set; }
        public IEnumerable<ProductsInCart> Products { get; set; }
    }

    public class ProductsInCart
    {
        public Guid SanPhamId { get; set; }
        public string TenSanPham { get; set; }
        public int SoLuong { get; set; }
        public long ThanhTien { get; set; }
    }
}
