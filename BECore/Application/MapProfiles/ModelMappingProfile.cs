using AutoMapper;

using Common.ViewModels.Authentication;
using Common.ViewModels.Category;
using Common.ViewModels.Store;

using Domain.Models;
using System.Collections.Generic;

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
        }
    }
}
