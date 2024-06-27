using DomainLayer.Entities;
using DomainLayer.MainInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainServices.Interface
{
    public interface ICommentService : IRepository<Comment>
    {
        // Definition private function model
    }
}
