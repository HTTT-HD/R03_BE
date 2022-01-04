using Common.Helpers;

using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;

using Infrastructure.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BECore.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration=configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();

            RegisterServices(services);
            #region =========== Config Hangfire =========
            var options = new MongoStorageOptions
            {
                MigrationOptions = new MongoMigrationOptions
                {
                    MigrationStrategy = new DropMongoMigrationStrategy(),
                    BackupStrategy = new NoneMongoBackupStrategy()
                }
            };

            var serverMongo = _configuration.GetSection(Constants.AuthConfig.MongoConnection + Constants.AuthConfig.ServerName).Value;
            var hangfireMongoDb = _configuration.GetSection(Constants.AuthConfig.MongoConnection + Constants.AuthConfig.HangfireDb).Value;

            services.AddHangfire(x => x.UseMongoStorage(serverMongo, hangfireMongoDb, options));
            services.AddHangfireServer();
            #endregion

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration.GetSection("JwtOptions:Issuer").Value,
                    ValidAudience = _configuration.GetSection("JwtOptions:Audience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetSection("JwtOptions:Secret").Value))
                };
            });

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "BECore.Api",
                    Description = "Swagger",
                    Contact = new OpenApiContact
                    {
                        Name = "",
                    },
                    License = new OpenApiLicense
                    {
                        Name = "",
                    }
                });
                s.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       },
                       Scheme = "oauth2",
                       Name = "Bearer",
                       In = ParameterLocation.Header,

                     },
                       new List<string>()
                   }
                });
                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"Nhập hộ cái token vào cái",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BECore v1"));
            }

            #region Hangfire
            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire");
            BackgroundJob.Enqueue(() => Console.WriteLine("Wellcome to hangfire with mongodb!"));
            #endregion

            app.UseCors(x => x
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            string folderUpload = Path.Combine(env.ContentRootPath, Constants.AuthConfig.FolderFileDefault);
            if (!Directory.Exists(folderUpload))
            {
                Directory.CreateDirectory(folderUpload);
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void RegisterServices(IServiceCollection services)
        {
            DependencyContainer.RegisterServices(services);
        }
    }
}
