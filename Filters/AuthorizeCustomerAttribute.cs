using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MCBA_Web.Models;

namespace MCBA_Web.Filters
{
    public class AuthorizeCustomerAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Bonus Material: Implement global authorisation check.
            // Skip authorisation check if the [AllowAnonymous] attribute is present.
            // Another technique to perform the check: x.GetType() == typeof(AllowAnonymousAttribute)
            //if(context.ActionDescriptor.EndpointMetadata.Any(x => x is AllowAnonymousAttribute))
            //    return;

            var customerID = context.HttpContext.Session.GetInt32(nameof(Customer.CustomerID));
            if(!customerID.HasValue)
                context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}
