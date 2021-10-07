using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using code.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace code
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
            services.AddControllers();

            // Add Distributed Redis Cache for Session
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = "localhost";
                options.InstanceName = "Session_";
            });

            // services.AddSwaggerGen(c =>
            // {
            //     c.SwaggerDoc("v1", new OpenApiInfo { Title = "code", Version = "v1" });
            // });

            services.AddSession(options =>
            {
                // 20 minutes later from last access your session will be removed.
                options.IdleTimeout = TimeSpan.FromSeconds(60);
                options.Cookie.Name = "ArraysCookie";
                options.Cookie.IsEssential = true;
            });

            // services.AddDistributedMemoryCache();
            // services.AddCaching();
            
            var dbConnectionString = Configuration.GetConnectionString("MvcMovieContext");
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(dbConnectionString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // app.UseSwagger();
                // app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "code v1"));
            }

            app.UseHttpsRedirection();
            app.UseSession();
            app.UseRouting();

            // app.UseAuthentication();
            // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
