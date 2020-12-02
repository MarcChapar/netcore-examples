using System;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Helpers.Security
{
    public class CommonAuthMiddleware
    {
        private string clientId;
        private string clientSecret;
        private IConfiguration config;

        public CommonAuthMiddleware(RequestDelegate next, IConfiguration config)
        {
            this.Next = next;
            this.config = config;

            this.clientId = config.GetValue<string>("ClientId");
            this.clientSecret = config.GetValue<string>("ClientSecret");
        }

        private RequestDelegate Next { get; set; }

        public async Task Invoke(HttpContext context)
        {
            var ignoredEndpoints = config.GetValue<string>("MiddlewareBypassEndpoints").Split(',');
            var path = context.Request.Path;
            if (Array.Exists(ignoredEndpoints, endpoint => endpoint.Equals(path, StringComparison.InvariantCultureIgnoreCase)))
            {
                await Next.Invoke(context);
                return;
            }

            if (context.Request.Headers.TryGetValue("Authorization", out StringValues token))
            {
                var authzValue = token.ToString();
                if (string.IsNullOrEmpty(authzValue) || !authzValue.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
                {
                    await SetUnauthorizedResponse(context);
                    return;
                }

                var creds = authzValue.Substring("Basic ".Length).Trim();
                string clientIdFromToken;
                string clientSecretFromToken;

                if (RetrieveCreds(creds, out clientIdFromToken, out clientSecretFromToken))
                {
                    context.User = new GenericPrincipal(new GenericIdentity(clientIdFromToken, "client"), new string[] { "client" });
                }
                else
                {
                    await SetUnauthorizedResponse(context);
                    return;
                }
            }
            else
            {
                await SetUnauthorizedResponse(context);
                return;
            }

            await Next.Invoke(context);
        }

        private bool RetrieveCreds(string credentials, out string clientId, out string clientSecret)
        {
            string pair;
            clientId = clientSecret = string.Empty;

            try
            {
                pair = Encoding.UTF8.GetString(Convert.FromBase64String(credentials));
            }
            catch (FormatException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            var ix = pair.IndexOf(':');
            if (ix == -1)
            {
                return false;
            }

            clientId = pair.Substring(0, ix);
            clientSecret = pair.Substring(ix + 1);

            return string.Compare(clientId, this.clientId) == 0 &&
                string.Compare(clientSecret, this.clientSecret) == 0;
        }

        private async Task SetUnauthorizedResponse(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized.");
        }
    }
}