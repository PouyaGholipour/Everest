using DomainLayer.DTOs.User;
using DomainLayer.Entities;
using DomainLayer.Helper;
using DomainServices.Interface;
using InfrastructureLayer.ApplicationDbContext;
using InfrastructureLayer.MainServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using DomainLayer.MainInterfaces;
using System.Linq.Expressions;
using DomainLayer.ServiceResults;

namespace DomainServices.Services
{
    public class UserService : Repository<User>, IUserService
    {
        private readonly EverestDataBaseContext _context;
        public IUnitOfWork _unitOfWork { get; }
        public UserService(EverestDataBaseContext context
                            , IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = (this._context ?? (EverestDataBaseContext)_context);
        }

        public bool IsExistUserName(string userName)
        {
            return _context.Users.Any(x => x.UserName == userName);

        }

        public bool IsExistEmail(string email)
        {
            return _context.Users.Any(x => x.Email == email);
        }

        public User LoginUser(LoginViewModel login)
        {
            // پیاده‌سازی متد ورود کاربر
            throw new NotImplementedException();
        }

        public void DeleteFromDataBase(int id)
        {
            // پیاده‌سازی حذف کاربر از پایگاه داده
            throw new NotImplementedException();
        }
        public async Task<string> AddUser(RegisterViewModel register)
        {
            string message = "";
            if (register == null)
                return message += "Null";

            if (IsExistEmail(register.Email) && IsExistUserName(register.UserName))
                return message += "UsernameEmailExist";

            if (IsExistUserName(register.UserName))
                return message += "UsernameExist";

            if (IsExistEmail(register.Email))
                return message += "EmailExist";

            var user = new User()
            {
                UserName = register.UserName,
                Email = register.Email,
                Password = register.Password,
                RegisterDate = DateTime.Now,
                ImageName = "Default.jpg",
            };

            await CreateAsync(user);
            return message;
        }
    }
}
