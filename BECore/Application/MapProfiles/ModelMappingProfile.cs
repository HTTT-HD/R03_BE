using AutoMapper;

using Common.ViewModels.Authentication;
using Common.ViewModels.Category;
using Common.ViewModels.Order;
using Common.ViewModels.Product;
using Common.ViewModels.Store;
using Domain.Models;

namespace Application.MapProfiles
{
    public class ModelMappingProfile : Profile
    {
        public ModelMappingProfile()
        {
            // News
            CreateMap<IdentityViewModel, ThanhVien>();
            CreateMap<ThanhVien, UserESViewModel>(); 
            CreateMap<StoreViewModel, CuaHang>();
            CreateMap<CategoryViewModel, DanhMuc>();
            CreateMap<ProductViewModel, SanPham>();
            CreateMap<DonHang, OrderDetail>();
            CreateMap<ChiTietDonHang, ChiTiet>();
        }
    }
}
