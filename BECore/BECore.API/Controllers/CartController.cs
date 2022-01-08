using Application.CartServices;
using Common.Utils;
using Common.ViewModels.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BECore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;

        public CartController(ICartService service)
        {
            _service = service;
        }

        [HttpPost("add-product")]
        public async Task<ServiceResponse> AddProductToCart(AddProductToCart model)
        {
            return await _service.AddProductToCart(model);
        }

        [HttpDelete("remove-cart-in-store")]
        public async Task<ServiceResponse> RemoveGioHang(Guid cuaHangId)
        {
            return await _service.RemoveGioHang(cuaHangId);
        }

        [HttpDelete("remove-product-in-cart")]
        public async Task<ServiceResponse> RemoveProduct(Guid cuaHangId, Guid sanPhamId)
        {
            return await _service.RemoveProduct(cuaHangId, sanPhamId);
        }

        [HttpPost("change-quantity")]
        public async Task<ServiceResponse> ChangeQuantity(AddProductToCart model)
        {
            return await _service.ChangeQuantity(model);
        }

        [HttpGet("list-product-in-sotre")]
        public async Task<ServiceResponse> GetAll(Guid cuaHangId)
        {
            return await _service.GetAll(cuaHangId);
        }
    }
}
