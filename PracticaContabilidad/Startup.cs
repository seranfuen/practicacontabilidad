using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PracticaContabilidad.Model;
using PracticaContabilidad.Model.Maintenance;
using PracticaContabilidad.Model.Repositories;
using PracticaContabilidad.Model.UnitOfWork;

namespace PracticaContabilidad
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute("DefaultRoute",
                    "{controller=Home}/{action=Index}/{id:int?}");
            });
        }

        // This method gets called by the runtime. Usfe this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDbContext<ContabilidadDbContext>(builder =>
                builder.UseSqlServer(Configuration.GetConnectionString("ContabilidadConnection")));
            services.AddScoped<IContabilidadSeeder, DevelopmentContabilidadSeeder>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IJournalEntryGroupRepository, JournalEntryGroupRepository>();
            services.AddTransient<IJournalEntryGroupPresetRepository, JournalEntryGroupPresetRepository>();
            services.AddAutoMapper(cfg => { }, typeof(Startup));
            services.AddSingleton<ILedgerEntryUoWFactory, LedgerEntryUoWFactory>();
        }
    }
}