﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace PracticaContabilidad
{
    public class Program
    {
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>().UseDefaultServiceProvider(options => options.ValidateScopes = true);
        }

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }
    }
}