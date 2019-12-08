using System.Text;
using AutoMapper;
using Carter;
using CoreBase.Extensions;
using CoreBase.Persistance;
using CoreBase.Persistance.finders;
using CoreBase.Services;
using CoreBase.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace CoreBase
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional : true, reloadOnChange : true);

            Configuration = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Entity framework
            services.AddDbContext<DatabaseContext>(options =>
                options.UseLazyLoadingProxies()
                .UseMySql(Configuration.GetConnectionString("DefaultConnection")));

            //Settings configuration
            var authSettingSection = Configuration.GetSection("AuthSettings");
            services.Configure<AuthSettings>(authSettingSection);

            //Service and finders
            services.AddScoped<IUserFinder, UserFinder>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBaseModuleService, BaseModuleService>();

            services.AddCarter();

            services.AddAutoMapper(typeof(Startup));

            //JWT config
            var authSettings = authSettingSection.Get<AuthSettings>();
            var key = Encoding.ASCII.GetBytes(authSettings.SecretKey);

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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DatabaseContext BaseContext)
        {
            if (env.IsEnvironment("Development"))
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseErrorHandlerMiddleware();
            app.UseDbContextMiddleware();
            app.UseRouting();
            app.UseAuthentication();
            app.UseEndpoints(builder => builder.MapCarter());
        }

    }
}
