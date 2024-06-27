using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.MainInterfaces
{
    public interface IUnitOfWork : IDisposable
    {

        void Commit();
        Task<int> CommitAsync();
    }
}
