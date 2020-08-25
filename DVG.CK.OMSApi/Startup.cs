using System.Linq;
using System.Text.Json;
using Base.Logging.LoggingMiddlewares;
using DVG.WIS.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DVG.CK.OMSApi
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
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddControllers();
            services.AddMvc().AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            IoC.RegisterTypes(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<ProcessExceptionMiddleware>();
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            //app.UseMiddleware<RetrieveUserMiddleware>();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public class SnakeCasePropertyNamingPolicy : JsonNamingPolicy
        {
            public override string ConvertName(string name)
            {
                return string.Concat(name.Select((character, index) =>
                        index > 0 && char.IsUpper(character)
                            ? "_" + character
                            : character.ToString()))
                    .ToLower();
            }
        }
    }
}
