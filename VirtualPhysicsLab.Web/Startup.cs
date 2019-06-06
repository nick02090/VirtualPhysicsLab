using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VirtualPhysicsLab.Web.Data;
using VirtualPhysicsLab.Web.Interfaces;
using VirtualPhysicsLab.Web.Repositories;
using VirtualPhysicsLab.Web.Services;
using VirtualPhysicsLab.Web.Settings;

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


            #region App Settings
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddSingleton<IAppSettings>(appSettings);
            #endregion

            #region Connection Settings
            var connectionSettings = new ConnectionSettings
            {
                ConnectionString = Configuration.GetConnectionString("VPL"),
                DbConnectionString = Configuration.GetConnectionString("DB")
            };

            services.AddSingleton<IConnectionSettings>(connectionSettings);
            #endregion

            #region Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMeshRepository, MeshRepository>();
            services.AddScoped<IExperimentRepository, ExperimentRepository>();
            services.AddScoped<IExperimentSettingsRepository, ExperimentSettingsRepository>();
            services.AddScoped<IMeshSettingsRepository, MeshSettingsRepository>();
            #endregion

            #region Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMeshService, MeshService>();
            services.AddScoped<IExperimentService, ExperimentService>();
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
