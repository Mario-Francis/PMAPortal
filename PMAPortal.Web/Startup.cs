using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PMAPortal.Web.Data;
using PMAPortal.Web.Data.Repositories;
using PMAPortal.Web.Data.Repositories.Implementations;
using PMAPortal.Web.DTOs;
using PMAPortal.Web.Middlewares;
using PMAPortal.Web.Service.Implementations;
using PMAPortal.Web.Services;
using PMAPortal.Web.Services.Implementations;
using PMAPortal.Web.UIServices;
using System;
using System.IO;

namespace PMAPortal.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(480);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.Name = "PMAPortal.Session";
            });

            services.AddAuthentication(Constants.AUTH_COOKIE_ID)
                .AddCookie(Constants.AUTH_COOKIE_ID,
                    options =>
                    {
                        options.LoginPath = "/Auth";
                        options.LogoutPath = "/Auth/Logout";
                    });

           

            services.AddDbContext<AppDbContext>(options => options.UseLazyLoadingProxies()
           .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IApplicationRepo, ApplicationRepo>();

            // services$(
            services.AddScoped(typeof(ILoggerService<>), typeof(LoggerService<>));
            services.AddScoped<IListService, ListService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IApplicationService, ApplicationService>();
            services.AddScoped<IMeterService, MeterService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IAreaService, AreaService>();
            services.AddScoped<IHouseTypeService, HouseTypeService>();
            services.AddScoped<IApplianceService, ApplianceService>();
            services.AddScoped<IPetService, PetService>();

            // ui services
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IDropdownService, DropdownService>();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddHttpClient();

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var path = Directory.GetCurrentDirectory();
            loggerFactory.AddFile($"{path}\\Logs\\Log.txt", isJson: true, outputTemplate: "=========================> {Timestamp:o} {RequestId,13} [{Level:u3}] <========================={NewLine} {Message} ({EventId:x8}){NewLine}{Exception} {NewLine}==========================================================================={NewLine}");


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();
            app.UseSession();

            app.UseAuthentication();
            app.UseMiddleware<SessionMiddleware>();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
