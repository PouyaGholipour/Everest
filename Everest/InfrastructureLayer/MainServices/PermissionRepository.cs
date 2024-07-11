using DomainLayer.Entities;
using DomainLayer.MainInterfaces;
using InfrastructureLayer.ApplicationDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.MainServices
{
    public class PermissionRepository : Repository<Role> , IPermissionRepository
    {
        private readonly EverestDataBaseContext _context;
        public IUnitOfWork _UnitOfWork { get; set; }
        public PermissionRepository(EverestDataBaseContext everestDataBase
                             , IUnitOfWork unitOfWork) : base(everestDataBase, unitOfWork)
        {
            _context = everestDataBase;
        }
        public void AddRoleToUser(List<int> RoleIds, int userId)
        {
            var roleUsers = RoleIds.Select(roleId => new RoleUser
            {
                UserId = userId,
                RoleId = roleId
            }).ToList();

            _context.RoleUsers.AddRange(roleUsers);
        }
        public void EditUserRole(int userId, List<int> RoleIds)
        {
            var userRoles = _context.RoleUsers.Where(x => x.UserId == userId).ToList();
            _context.RoleUsers.RemoveRange(userRoles);

            var newRoleUsers = RoleIds.Select(roleId => new RoleUser
            {
                UserId = userId,
                RoleId = roleId
            }).ToList();

            _context.RoleUsers.AddRange(newRoleUsers);

        }
    }
}
