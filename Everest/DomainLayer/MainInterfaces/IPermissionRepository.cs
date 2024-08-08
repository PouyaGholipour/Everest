using DomainLayer.DTOs.Role;
using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.MainInterfaces
{
    public interface IPermissionRepository : IRepository<Role>
    {
        //// Definition private function model

        void AddRoleToUser(List<int> RoleId, int userId);
        void EditUserRole(int userId, List<int> RoleIds);
        int CreateRole(CreateRoleViewModel createRole);
        List<CreateRoleViewModel> GetRoles();
        List<Role> GetRoleList();
        CreateRoleViewModel ShowRoleForEditMode(int id);
        void EditRole(CreateRoleViewModel createRole);
        void DeleteRole(int id);
    }
}
