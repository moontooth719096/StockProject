using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonService.DBClass;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StockAPI.Controllers.Services;

namespace StockAPI
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
            services.AddCors(options =>
            {
                // CorsPolicy 是自訂的 Policy 名稱
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.WithOrigins("http://192.168.204.131:4200","http://localhost:4200")
                             .AllowAnyHeader()
                             .AllowAnyMethod()
                             .AllowCredentials();
                });
            });
            //public void ConfigureServices(IServiceCollection services)
            //{
            //    services.AddCors(options =>
            //    {
            //        // CorsPolicy 是自訂的 Policy 名稱
            //        options.AddPolicy("CorsPolicy", policy =>
            //        {
            //            policy.WithOrigins("http://localhost:3000")
            //                  .AllowAnyHeader()
            //                  .AllowAnyMethod()
            //                  .AllowCredentials();
            //        });
            //    });
            //    // ...
            //}
            services.AddControllers();
            services.AddHttpClient();
            services.AddSingleton<StockIDataService>();
            services.AddSingleton<StockInfoDB>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
