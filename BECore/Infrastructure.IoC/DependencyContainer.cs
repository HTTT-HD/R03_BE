using Application.MapProfiles;
using Application.PermissionServices;
using Application.RoleServices;
using Application.AuthenticationServices;

using AutoMapper;
using Domain.Data;
using Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;


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
        }
    }
}
