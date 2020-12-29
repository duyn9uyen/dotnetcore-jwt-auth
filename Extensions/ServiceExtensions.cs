using dotnetcore_jwt_auth.Models;
using dotnetcore_jwt_auth.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetcore_jwt_auth.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
                    //.AllowCredentials());
            });
        }

        public static void ConfigureJwtAuth(this IServiceCollection services)
        {
            services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "http://localhost:5000", //This much match what the API passes back in the AuthController.Login()
                    ValidAudience = "http://localhost:5000",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345")) //Store as environment variable?
                };
            });

        }

        public static void ConfigureEfCore(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<UserContext>(opts => opts.UseSqlServer(config["ConnectionString:UserDB"]));
        }

        public static void ConfigureEfCoreInMemoryDb(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<UserContext>(opts => opts.UseInMemoryDatabase("UserContextDb"));
        }

        public static void ConfigureModels(this IServiceCollection services)
        {
            services.AddTransient<ITokenService, TokenService>();
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "AccountOwner API",
                    Version = "v1"
                });
            });
        }

        // public static void ConfigureIISIntegration(this IServiceCollection services)
        // {
        //     services.Configure<IISOptions>(options =>
        //     {

        //     });
        // }

        // public static void ConfigureLoggerService(this IServiceCollection services)
        // {
        //     services.AddSingleton<ILoggerManager, LoggerManager>();
        // }

        // public static void ConfigureMySqlContext(this IServiceCollection services, IConfiguration config)
        // {
        //     var connectionString = config["mysqlconnection:connectionString"];
        //     services.AddDbContext<RepositoryContext>(o => o.UseMySql(connectionString));
        // }

        // public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        // {
        //     services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        // }

    }
}