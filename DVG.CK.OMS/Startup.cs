using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DVG.WIS.Core.Enums;
using DVG.WIS.PublicModel;
using DVG.WIS.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using DVG.WIS.Caching.Interfaces;
using DVG.WIS.Caching;

namespace DVG.CK.OMS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AppSettings.Instance.SetConfiguration(Configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //https
            var builder = services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                //config https
                // options.Filters.Add(new RequireHttpsAttribute());
            });

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
            });

            // Configure Identity
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                //options.Password.RequireDigit = true;
                //options.Password.RequiredLength = 6;
                //options.Password.RequireNonAlphanumeric = false;
                //options.Password.RequireUppercase = false;
                //options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.SlidingExpiration = true;
            });

            //services.AddIdentity<UserLogin, IdentityRole>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = "/dang-nhap";
                        options.AccessDeniedPath = "/access-denied";
                        options.ExpireTimeSpan = TimeSpan.FromDays(7);

                    });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole(
                    UserTypeEnum.Admin.GetHashCode().ToString())
                );
                options.AddPolicy("ManagerRole", policy => policy.RequireRole(
                    UserTypeEnum.Manager.GetHashCode().ToString(),
                    UserTypeEnum.Admin.GetHashCode().ToString()
                    ));
                options.AddPolicy("KitchenManagerRole", policy => policy.RequireRole(
                    UserTypeEnum.KitchenManager.GetHashCode().ToString(),
                    UserTypeEnum.Admin.GetHashCode().ToString()
                    ));
                options.AddPolicy("CustomerServiceRole", policy => policy.RequireRole(
                    UserTypeEnum.CustomerService.GetHashCode().ToString(),
                    UserTypeEnum.Manager.GetHashCode().ToString(),
                    UserTypeEnum.Admin.GetHashCode().ToString()
                    ));
                options.AddPolicy("DestroyOrderRole", policy => policy.RequireRole(
                    UserTypeEnum.Manager.GetHashCode().ToString(),
                    UserTypeEnum.Admin.GetHashCode().ToString(),
                    UserTypeEnum.KitchenManager.GetHashCode().ToString(),
                    UserTypeEnum.CustomerService.GetHashCode().ToString()
                    ));
                options.AddPolicy("KitchenRole", policy => policy.RequireRole(
                    UserTypeEnum.Kitchen.GetHashCode().ToString(),
                    UserTypeEnum.Admin.GetHashCode().ToString()
                    ));
                options.AddPolicy("CashierRole", policy => policy.RequireRole(
                    UserTypeEnum.Cashier.GetHashCode().ToString(),
                    UserTypeEnum.Admin.GetHashCode().ToString()
                    ));
            });

            //redis cache
            //var redisCacheSection = Configuration.GetSection("Cache").GetSection("RedisCache");
            //services.AddDistributedRedisCache(options =>
            //{
            //    options.Configuration = redisCacheSection.GetValue<string>("Server") + ":" + redisCacheSection.GetValue<string>("Port");
            //    options.InstanceName = redisCacheSection.GetValue<string>("InstanceName");
            //});


            //services.AddDataProtection()
            //    .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
            //    {
            //        EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
            //        ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
            //    });

            // Add application services.


            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            });

            IoC.RegisterTypes(services);


            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc()
                //.AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                })
                .AddRazorPagesOptions(options =>
                {
                    //options.RootDirectory = "/Pages";
                    options.Conventions.AddPageRoute("/FileManager/Index", "FileManager");
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSession();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});
            app.UseMvc(RouteConfig.RegisterRoutes);
        }
    }
}
