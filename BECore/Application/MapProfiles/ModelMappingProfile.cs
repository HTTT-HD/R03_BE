using AutoMapper;

using Common.ViewModels.Authentication;

using Domain.Models;

namespace Application.MapProfiles
{
    public class ModelMappingProfile : Profile
    {
        public ModelMappingProfile()
        {
            // News
            CreateMap<IdentityViewModel, ThanhVien>();
        }
    }
}
