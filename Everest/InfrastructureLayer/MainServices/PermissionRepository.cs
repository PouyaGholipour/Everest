using DomainLayer.DTOs.Role;
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
        public IUnitOfWork _unitOfWork { get; set; }
        public PermissionRepository(EverestDataBaseContext everestDataBase
                             , IUnitOfWork unitOfWork) : base(everestDataBase, unitOfWork)
        {
            _context = everestDataBase;
            _unitOfWork = unitOfWork;
        }

        public List<Role> GetRoleList()
        {
            var permmissions = GetAll();
            return permmissions;
        }
        public void AddRoleToUser(List<int> RoleIds, int userId)
        {
            var roleUsers = RoleIds.Select(roleId => new RoleUser
            {
                UserId = userId,
                RoleId = roleId
            }).ToList();

            _context.RoleUsers.AddRange(roleUsers);
            _unitOfWork.Commit();
        }
        public void EditUserRole(int userId, List<int> RoleIds)
        {
            var userRoles = _context.RoleUsers.Where(x => x.UserId == userId).ToList();
            _context.RoleUsers.RemoveRange(userRoles);
            _unitOfWork.Commit();

            var newRoleUsers = RoleIds.Select(roleId => new RoleUser
            {
                UserId = userId,
                RoleId = roleId
            }).ToList();

            _context.RoleUsers.AddRange(newRoleUsers);
            _unitOfWork.Commit();

        }

        public int CreateRole(CreateRoleViewModel createRole)
        {
            Role role = new Role()
            {
                RoleTitle = createRole.Title,
            };
            Create(role);
            return role.RoleId;
        }

        public List<CreateRoleViewModel> GetRoles()
        {
            var roleList = GetAll();
            var roleListModel = roleList.Select(x => new CreateRoleViewModel
            {
                Id = x.RoleId,
                Title = x.RoleTitle
            }).ToList();

            return roleListModel;
        }

        public CreateRoleViewModel ShowRoleForEditMode(int id)
        {
            var role = GetById(id);
            CreateRoleViewModel createRole = new CreateRoleViewModel()
            {
                Id = role.RoleId,
                Title = role.RoleTitle,
            };
            return createRole;
        }

        public void EditRole(CreateRoleViewModel createRole)
        {
            var role = GetById(createRole.Id);
            role.RoleTitle = createRole.Title;
            Update(role);
        }

        public void DeleteRole(int id)
        {
            var role = GetById(id);
            role.IsDelete = true;
            Update(role);
        }
    }
}
