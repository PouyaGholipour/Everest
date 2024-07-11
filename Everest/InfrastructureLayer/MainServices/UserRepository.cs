using DomainLayer.DTOs.User;
using DomainLayer.Entities;
using DomainLayer.Helper;
using DomainLayer.MainInterfaces;
using InfrastructureLayer.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.MainServices
{
    public class UserRepository : Repository<User>, IUserRepositoy
    {
        private readonly EverestDataBaseContext _Context;
        public IUnitOfWork _UnitOfWork { get; set; }
        public UserRepository(EverestDataBaseContext everestDataBase
                             ,IUnitOfWork unitOfWork) : base(everestDataBase, unitOfWork)
        {
            _Context = everestDataBase;
        }

        public async Task<User> GetUserWithRolesByIdAsync(int id)
        {
            return await _Context.Users.Include(x => x.RoleUsers).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
