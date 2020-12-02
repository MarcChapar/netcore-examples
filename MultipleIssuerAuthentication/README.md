# Azure Multiple Issuer Authentication

This configuration will let your API receive tokens issued by either Azure AD or Azure ADB2C.

To use it just add the following line n the Startup file inside ConfigureServices():
           ```
            services.ConfigureMultipleAuthenticationTokens(Configuration);
           ```
For it to work you need to have certain keys defined and completed with the proper information inside appsettings. These are:
            ```json
                "AzureAdB2C": {
                    "Instance": "",
                    "ClientId": "",
                    "Domain": "",
                    "SignUpSignInPolicyId": ""
                },
                "AzureAd": {
                    "Instance": "",
                    "ClientId": "",
                    "Domain": "",
                    "TenantId": ""
                }
            ```
Also, you will need to install the following packages:
    Microsoft.AspNetCore.Authentication.AzureAD.UI;
    Microsoft.AspNetCore.Authentication.AzureADB2C.UI;