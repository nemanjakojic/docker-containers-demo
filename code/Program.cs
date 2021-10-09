using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace code
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            // WebHost.CreateDefaultBuilder(args)
            //     .UseKestrel(options => {
            //         options.Listen(IPAddress.Any, 8443, listenOptions =>
            //         {
            //             listenOptions.UseHttps(
            //                 fileName: "/home/ubuntu/.aspnet/https/aspnetapp.pfx", 
            //                 password: "Test.123!");
            //         });
            //     })
            //     .UseStartup<Startup>();
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
