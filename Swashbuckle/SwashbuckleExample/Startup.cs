using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace SwashbuckleExample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private const string ApiName = "SwashbuckleExample";
        private const string ApiTitle = "Swashbuckle Test Api";
        private const string BasePath = "/Swashbuckle";
        private const string ApiDesc = "Swashbuckle implementation example";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c => SwashbuckleSetup.SwaggerConfiguration(c, ApiName, ApiTitle, ApiDesc, Configuration));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    var paths = new OpenApiPaths();
                    foreach (var path in swaggerDoc.Paths)
                    {
                        paths.Add(BasePath + path.Key, path.Value);
                    }

                    swaggerDoc.Paths = paths;

                    var servers = new OpenApiServer
                    {
                        Url = httpReq.Host + BasePath,
                    };

                    swaggerDoc.Servers.Add(servers);
                });
            });
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swashbuckle Test API V1"); });
        }
    }
}
