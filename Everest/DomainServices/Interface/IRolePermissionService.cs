using DomainLayer.Entities;
using DomainLayer.MainInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainServices.Interface
{
    public interface IRolePermissionService : IRepository<RolePermission>
    {
        void AddPermissionToRole(int roleId, List<int> permissions);
        List<int> PermissionsInRole(int roleId);
        void UpdateRolePermission(int roleId, List<int> permissions);
    }
}
