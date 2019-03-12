using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtualPhysicsLab.Web.Data;

namespace VirtualPhysicsLab.Web
{
    public class Startup
    {
        public IHostingEnvironment CurrentEnvironment { get; }
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
                });

            services.AddDbContext<VPLContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DB")));

            #region Repositories
            // Example: services.AddScoped<IXRepository, XRepository>();
            #endregion

            #region Services
            // Example: services.AddScoped<IXService, XService>();
            #endregion

            services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                    options.HttpsPort = 443;
                });

            services.AddResponseCompression();
            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
            }

            app.UseStaticFiles();

            app.UseResponseCompression();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Main}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Main", action = "Index" });
            });
        }
    }
}
