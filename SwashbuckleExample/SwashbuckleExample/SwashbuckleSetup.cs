using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwashbuckleExample
{
    public class SwashbuckleSetup
    {
        public static void SwaggerConfiguration(SwaggerGenOptions options, string apiName, string swaggerApiTitle, string swaggerDescription, IConfiguration configuration)
        {
            var email = configuration.GetSection("Swagger:Email").Get<string>();
            var contactUri = configuration.GetSection("Swagger:ContactUri").Get<string>();

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = swaggerApiTitle,
                Description = swaggerDescription,
                Contact = new OpenApiContact
                {
                    Name = "Contact",
                    Email = email,
                    Url = new Uri(contactUri),
                },
                License = new OpenApiLicense
                {
                    Name = "All Rights Reserved",
                },
            });

            options.EnableAnnotations();
            var xmlFile = $"{apiName}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        }

        public static void SwaggerConfigurationWithApiBase(SwaggerGenOptions options, string apiName, string swaggerApiTitle, string swaggerDescription, IConfiguration configuration)
        {
            SwaggerConfiguration(options, apiName, swaggerApiTitle, swaggerDescription, configuration);

            var apiBaseXmlFile = $"{ApiBaseXmlFileName()}.xml";
            var apiBaseXmlPath = Path.Combine(ApiBaseXmlFilePath(), apiBaseXmlFile);
            options.IncludeXmlComments(apiBaseXmlPath);
        }

        private static string ApiBaseXmlFileName()
        {
            return AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().Name.Contains("APIBase")).FirstOrDefault().GetName().Name;
        }

        private static string ApiBaseXmlFilePath()
        {
            return Directory.GetParent(AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().Name.Contains("APIBase")).FirstOrDefault().Location).FullName;
        }
    }
}
