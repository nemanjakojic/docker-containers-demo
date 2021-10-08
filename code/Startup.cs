using code.Core;
using code.Core.Application;
using code.Core.Operations;
using code.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                options.Configuration = Configuration.GetSection("Redis").GetConnectionString("RedisCache");
                options.InstanceName = "Session_";
            });

            services.AddSession(options =>
            {
                options.IdleTimeout = AppConstants.DefaultSessionIdleTimeout;
                options.Cookie.Name = AppConstants.AppCookieName;
                options.Cookie.IsEssential = true;
            });

            // Configure injected dependencies
            var dbConnectionString = Configuration.GetConnectionString("AppDbContext");
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(dbConnectionString));
            services.AddTransient<IHashGenerator, BCryptHashGenerator>();
            services.AddTransient<IDateTimeProvider, DefaultDateTimeProvider>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IOperationProvider, OperationProvider>();
            services.AddTransient<LogInOperation>();
            services.AddTransient<LogOutOperation>();
            services.AddTransient<SignUpOperation>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
