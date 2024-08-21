using DomainLayer.Entities;
using DomainLayer.MainInterfaces;
using DomainServices.Interface;
using InfrastructureLayer.ApplicationDbContext;
using InfrastructureLayer.MainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainServices.Services
{
    public class RolePermissionService : Repository<RolePermission>, IRolePermissionService
    {
        private readonly EverestDataBaseContext _context;
        public IUnitOfWork _unitOfWork { get; set; }
        public RolePermissionService(EverestDataBaseContext everestDataBase
                             , IUnitOfWork unitOfWork) : base(everestDataBase, unitOfWork)
        {
            this._context = (this._context ?? (EverestDataBaseContext)everestDataBase);
            _unitOfWork = unitOfWork;
        }

        public void AddPermissionToRole(int roleId, List<int> permissions)
        {
            foreach (var permission in permissions)
            {
                Create(new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = permission
                });
            }
        }

        public List<int> PermissionsInRole(int roleId)
        {
            var permissionRole = GetListAsync(p => p.RoleId == roleId).Result;
            return permissionRole.Select(p => p.PermissionId).ToList();
        }

        public void UpdateRolePermission(int roleId, List<int> permissions)
        {
            var permissionRole = GetListAsync(p => p.RoleId == roleId).Result;
            RemoveRange(permissionRole);

            AddPermissionToRole(roleId, permissions);
        }

        public bool CheckPermission(int permissionId, string userName)
        {
            // Give Data from database context
            int userId = _context.Users.FirstOrDefault(x => x.UserName == userName).Id;

            List<int> userRolesIds = _context.RoleUsers.Where(x => x.UserId == userId)
                .Select(x => x.RoleId).ToList();

            if (!userRolesIds.Any()) return false;

            List<int> RolePermissionIds = _context.RolePermissions
                .Where(x => x.PermissionId == permissionId)
                .Select(x => x.RoleId).ToList();

            return RolePermissionIds.Any(x => userRolesIds.Contains(x));
        }
    }
}
