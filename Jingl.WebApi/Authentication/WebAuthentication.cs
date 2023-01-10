using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Jingl.WebApi.Helper;

namespace Jingl.WebApi.Authentication
{
    public class WebAuthentication : IAuthorizationFilter
    {

        private readonly string realm;
       
        public AuthLogin AuthLog;

        public WebAuthentication(string realm = null)
        {
            this.realm = realm;
           

        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string authHeader = context.HttpContext.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic "))
            {
                // Get the encoded username and password
                var encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();

                // Decode from Base64 to string
                var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));

                // Split username and password
                var username = decodedUsernamePassword.Split(':', 2)[0];
                var password = decodedUsernamePassword.Split(':', 2)[1];

                // Check if login is correct
                if (IsAuthorized(username, password))
                {
                    return;
                }
            }

            // Return authentication type (causes browser to show login dialog)
            context.HttpContext.Response.Headers["WWW-Authenticate"] = "Basic";

            // Add realm if it is not null
            if (!string.IsNullOrWhiteSpace(realm))
            {
                context.HttpContext.Response.Headers["WWW-Authenticate"] += $" realm=\"{realm}\"";
            }

            // Return unauthorized
            context.Result = new UnauthorizedResult();
        }

        // Make your own implementation of this
        public bool IsAuthorized(string username, string password)
        {
            //var config =  IConfiguration();
            //string ValidUserName = HelperController.UserNameAccess();
            //string ValidPassword = HelperController.PasswordAccess();

            // Check that username and password are correct
            //return username.Equals(ValidUserName, StringComparison.InvariantCultureIgnoreCase)
            //        && password.Equals(ValidPassword);

            return username.Equals("BEFameoApp465", StringComparison.InvariantCultureIgnoreCase)
                  && password.Equals("J1i2n3g4l5");
        }
    }
}
