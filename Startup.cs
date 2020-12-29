using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using dotnetcore_jwt_auth.Models;
using Microsoft.EntityFrameworkCore;
using dotnetcore_jwt_auth.Services;
using dotnetcore_jwt_auth.Extensions;

namespace dotnetcore_jwt_auth
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // We moved custom configurations into the ServiceExtension.cs method to keep startup.cs clean
            services.ConfigureCors();
            services.ConfigureJwtAuth();
            //services.ConfigureEfCore(Configuration);
            services.ConfigureEfCoreInMemoryDb(Configuration);
            services.ConfigureModels();
            services.ConfigureSwagger();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection(); //method requires https clients

            //Use CORS method
            app.UseCors("EnableCORS");

            app.UseRouting();

            // jwt-auth
            // Do we know who you are?
            app.UseAuthentication();

            // No we know who you are, are you allowed?
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AccountOwner API V1");
            });
        }
    }
}
