
using Common.Utils;
using Common.ViewModels.Cart;

using System;
using System.Threading.Tasks;

namespace Application.CartServices
{
    public interface ICartService
    {
        Task<ServiceResponse> AddProductToCart(AddProductToCart model);
        Task<ServiceResponse> RemoveGioHang(Guid cuaHangId);
        Task<ServiceResponse> RemoveProduct(Guid cuaHangId, Guid sanPhamId);
        Task<ServiceResponse> ChangeQuantity(AddProductToCart model);
        Task<ServiceResponse> GetAll(Guid cuaHangId);
        Task<ServiceResponse> GetProductsInCart(Guid gioHangId);
    }
}
