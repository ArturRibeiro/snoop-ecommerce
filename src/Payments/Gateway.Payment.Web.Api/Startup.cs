using System;
using System.IO;
using Gateway.Payment.Cross.Bus;
using Gateway.Payment.Dependency.Injection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Code.Bus;
using Frameworker.Scorponok.AspNet.Mvc;
using Gateway.Payment.Web.Api.Middlewares;

namespace Gateway.Payment.Web.Api
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

                services.AddMediatR(typeof(Startup).Assembly);

                services.AddHttpContextAccessor();

                // Domain Bus (Mediator)
                services.AddScoped<IMediatorHandler, InMemoryBus>();

                NativeDependencyInjection.RegisterServices(services);

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
