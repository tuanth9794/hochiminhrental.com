using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CoreCnice
{
    public class Program
    {
        //    public static void Main(string[] args)
        //    {
        //        BuildWebHost(args).Run();
        //    }

        //    public static IWebHost BuildWebHost(string[] args) =>
        //        WebHost.CreateDefaultBuilder(args)
        //            .UseStartup<Startup>()
        //          .UseApplicationInsights()
        //            .Build();
        //}

        public static void Main(string[] args)
        {
            //try
            //{ 
            var host = new WebHostBuilder()
              .UseKestrel()
              .UseContentRoot(Directory.GetCurrentDirectory())
              .UseEnvironment("Development")
              .UseStartup<Startup>()
              .UseApplicationInsights()
               .UseIISIntegration()
              .Build();

            host.Run();
            //}
            //catch(Exception ex)
            //{
                
            //}
            //BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
             //.UseApplicationInsights()
                .Build();
    }
}
