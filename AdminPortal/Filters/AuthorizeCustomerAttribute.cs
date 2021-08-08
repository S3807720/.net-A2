using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MCBA_Models.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace MCBA_Web.Filters
{
    public class AuthorizeCustomerAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if(context.ActionDescriptor.EndpointMetadata.Any(x => x is AllowAnonymousAttribute))
                return;

            var customerID = context.HttpContext.Session.GetString(nameof(Customer.CustomerID));
            if(customerID == null)
                context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}
