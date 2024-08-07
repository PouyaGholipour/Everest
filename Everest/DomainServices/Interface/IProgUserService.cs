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
    public interface IProgUserService : IRepository<ProgUser>
    {
        Task<ServiceException> AddProgToUser(int userId, int progId);
        Task<ServiceException> RemoveProgUser(int userId, int progId);
    }
}
