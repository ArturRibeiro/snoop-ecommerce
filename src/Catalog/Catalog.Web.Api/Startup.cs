using System;
using System.IO;
using Catalog.Infrastructure;
using Catalog.Web.Api.App;
using Catalog.Web.Api.Filters;
using Frameworker.Scorponok.AspNet.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Code.Provider;

namespace Catalog.Web.Api
{
    public class Startup
    {
        public Startup(IWebHostEnvironment hostingEnvironment)
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(hostingEnvironment.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: true);

                builder.AddEnvironmentVariables();
                Configuration = builder.Build();
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                services.AddMvc(options =>
                {
                    options.AddNotificationAsyncResultFilter<NotificationAsyncResultFilter>(Configuration);
                }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                  .AddNewtonsoftJson();

                services.AddHttpContextAccessor();

                NativeDependencyInjection.RegisterServices(services);

                services.AddDbContext<CatalogContext>(options
                    => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

                services.AddSingleton<IDataConfigurationProvider>(_ => new DataConfigurationProvider(Configuration.GetConnectionString("DefaultConnection")));

            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting(routes =>
            {
                routes.MapControllers();
            });

            app.UseAuthorization();
        }

        private void Log(Exception e) => File.AppendAllText($"Log\\Log{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}.txt", e.ToString());
    }
}
