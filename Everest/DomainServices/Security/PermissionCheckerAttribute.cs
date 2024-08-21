using DomainServices.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DomainServices.Security
{
    public class PermissionCheckerAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private int _permission = 0;
        private IRolePermissionService _rolePermissionService;
        public PermissionCheckerAttribute(int permissionId)
        {
            _permission = permissionId;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _rolePermissionService = (IRolePermissionService)context.HttpContext.RequestServices.GetService(typeof(IRolePermissionService));
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                string userName = context.HttpContext.User.Identity.Name;
                if(!_rolePermissionService.CheckPermission(_permission, userName))
                    context.Result = new RedirectResult("/");
            }
            else
                context.Result = new RedirectResult("/Login");
        }
    }
}
