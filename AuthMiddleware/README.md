# Common Authentication Middleware

This file will allow you tu add a Basic Authentication layer to your project through a middleware.

It works by comparing credentials recieved through Basic Auth to the ones configured in appsettings. If they don't match or the authentication type sent is not Basic it will respond with 401 Unauthorized.

To use it in an API just add it as a Middleware in the Startup file (app.UseMiddleware<CommonAuthMiddleware>() inside Configure() method).