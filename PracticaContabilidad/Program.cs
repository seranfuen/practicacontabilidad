using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PracticaContabilidad.Model;
using PracticaContabilidad.Model.Maintenance;

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
            var webHost = CreateWebHostBuilder(args).Build();
            SeedDatabase(webHost);
            webHost.Run();
        }

        private static void SeedDatabase(IWebHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var initializer = services.GetService<IContabilidadSeeder>();
                    initializer.SeedContext(services.GetRequiredService<ContabilidadDbContext>());
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }
    }
}