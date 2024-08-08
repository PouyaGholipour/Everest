using DomainLayer.DTOs.Role;
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
    public class PermissionService : Repository<Permission>, IPermissionService
    {
        private readonly EverestDataBaseContext _context;
        public IUnitOfWork _unitOfWork { get; }
        public PermissionService(EverestDataBaseContext context
                            , IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = (this._context ?? (EverestDataBaseContext)context);
        }

        public List<Permission> GetPermissions()
        {
            var permissions = GetAll();
            return permissions;
        }
    }
}
