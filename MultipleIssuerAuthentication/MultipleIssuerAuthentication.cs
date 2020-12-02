using Helpers.Identity;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.AzureADB2C.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MultipleIssuerAuthentication
{
    public static class MultipleIssuerAuthentication
    {
        /// <summary>
        /// It configures authentication and authorization both for ADB2C and AzureAD, to allow both tokens to be used
        /// to authenticate on the APIs.
        /// </summary>
        /// <param name="services">IServiceCollection configured on the Startup file.</param>
        /// <param name="configuration">IConfiguration instance with content of appsettings.json file.</param>
        /// <remarks>appsettings.json file of the project should contain sections AzureAd and AzureAdB2C with corresponding
        /// configurations.</remarks>
        public static void ConfigureMultipleAuthenticationTokens(this IServiceCollection services, IConfiguration configuration)
        {
            var azureADOptions = configuration.GetSection("AzureAd").Get<AzureADOptions>();
            var azureADB2COptions = configuration.GetSection("AzureAdB2C").Get<AzureADB2COptions>();

            services.AddAuthentication(AzureADB2CDefaults.BearerAuthenticationScheme)
            .AddJwtBearer(AzureADB2CDefaults.BearerAuthenticationScheme, options =>
            {
                options.Audience = azureADB2COptions.ClientId;

                var baseUri = new Uri(azureADB2COptions.Instance);
                var pathBase = baseUri.PathAndQuery.TrimEnd('/');
                var domain = azureADB2COptions.Domain;
                var policy = azureADB2COptions.DefaultPolicy;

                options.Authority = new Uri(baseUri, new PathString($"{pathBase}/{domain}/{policy}/v2.0")).ToString();
            })
            .AddJwtBearer(AzureADDefaults.JwtBearerAuthenticationScheme, options =>
            {
                options.Audience = azureADOptions.ClientId;
                options.Authority = new Uri(new Uri(azureADOptions.Instance), azureADOptions.TenantId + "/v2.0").ToString();
                options.TokenValidationParameters.ValidAudiences = new string[]
                {
                    options.Audience, $"api://{options.Audience}",
                };
                options.TokenValidationParameters.IssuerValidator = AadIssuerValidator.GetIssuerValidator(options.Authority).Validate;
            });

            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                    AzureADB2CDefaults.BearerAuthenticationScheme,
                    AzureADDefaults.JwtBearerAuthenticationScheme);
                defaultAuthorizationPolicyBuilder =
                    defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });
        }
    }
}
