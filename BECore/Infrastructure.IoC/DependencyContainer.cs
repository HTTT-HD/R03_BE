using Application.MapProfiles;
using Application.PermissionServices;
using Application.RoleServices;
using Application.AuthenticationServices;
using AutoMapper;
using Domain.Data;
using Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ElasticSearch.IServices;
using ElasticSearch.Services;
using Application.StoreServices;
using Application.CategoryServices;
using Application.ProductServices;
using Application.CartServices;

namespace Infrastructure.IoC
{
    public static class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ModelMappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IMongoContext, MongoContext>();
            services.AddScoped<IMongoRepository, MongoRepository>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICartService, CartService>();


            services.AddScoped<IElasticSearchRepository, ElasticSearchRepository>();
            services.AddScoped<IES_ThanhVien, ES_ThanhVien>();
        }
    }
}
