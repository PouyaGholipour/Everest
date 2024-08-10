using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Security
{
    public class PermissionCheckerAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private int _permission = 0;
        public PermissionCheckerAttribute(int permissionId)
        {
            _permission = permissionId;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {

            }
            else
            {
                context.Result = new RedirectResult("/Login");
            }
        }
    }
}
