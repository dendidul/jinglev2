using System;
using Microsoft.AspNetCore.Mvc;

namespace Jingl.WebApi.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]

    public class WebAutheticationAttribute : TypeFilterAttribute
    {
        public WebAutheticationAttribute(string realm = null)
           : base(typeof(WebAuthentication))
        {
            Arguments = new object[]
            {
                realm
            };
        }

    }
}
