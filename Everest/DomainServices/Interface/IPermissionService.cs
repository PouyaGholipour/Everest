using DomainLayer.DTOs.Role;
using DomainLayer.Entities;
using DomainLayer.MainInterfaces;
using DomainServices.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainServices.Interface
{
    public interface IPermissionService : IRepository<Permission>
    {
        List<Permission> GetPermissions();
    }
}
