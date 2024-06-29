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
using DomainLayer.Enums;
using TopLearn.Core.Generators;
using TopLearn.Core.Convertors;
using TopLearn.Core.Senders;

namespace DomainServices.Services
{
    public class UserService : Repository<User>, IUserService
    {
        private readonly EverestDataBaseContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IViewRenderService _viewRender;
        public IUnitOfWork _unitOfWork { get; }
        public UserService(EverestDataBaseContext context,
                           IUnitOfWork unitOfWork,
                           IHttpContextAccessor httpContextAccessor,
                           IViewRenderService viewRender) : base(context, unitOfWork)
        {
            this._context = (this._context ?? (EverestDataBaseContext)context);
            _httpContextAccessor = httpContextAccessor;
            _viewRender = viewRender;
        }

        public bool IsExistUserName(string userName)
        {
            return _context.Users.Any(x => x.UserName == userName);

        }

        public bool IsExistEmail(string email)
        {
            return _context.Users.Any(x => x.Email == email);
        }

        public async Task<ClientMessageType> LoginUser(LoginViewModel login)
        {
            var message = ClientMessageType.Default;
                
            string email = FixText.FixEmail(login.Email);
            string password = login.Password;

            var user = _context.Users.FirstOrDefault(x => x.Email == email && x.Password == password);

            if(user == null)
                return message = ClientMessageType.Null;

            if (user.IsActive)
            {
                var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName),
                    };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var properties = new AuthenticationProperties()
                {
                    IsPersistent = login.RememberMe
                };
                await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);
                return message = ClientMessageType.Success;
            }
            else
                return message = ClientMessageType.IsNotActive;
        }

        public void DeleteFromDataBase(int id)
        {
            // پیاده‌سازی حذف کاربر از پایگاه داده
            throw new NotImplementedException();
        }
        public async Task<ClientMessageType> AddUser(RegisterViewModel register)
        {
            var message = ClientMessageType.Default;
            if (register == null)
                return message = ClientMessageType.Null;

            if (IsExistEmail(register.Email) && IsExistUserName(register.UserName))
                return message = ClientMessageType.UsernameEmaliIsExist;

            if (IsExistUserName(register.UserName))
                return message = ClientMessageType.UsernameIsExist;

            if (IsExistEmail(register.Email))
                return message = ClientMessageType.EmailIsExist;

            var user = new User()
            {
                UserName = register.UserName,
                Email = FixText.FixEmail(register.Email),
                Password = register.Password,
                RegisterDate = DateTime.Now,
                ImageName = "Default.jpg",
                ActiveCode = NameGenerator.GenerateUniqCode()
            };

            string body = _viewRender.RenderToStringAsync("_ActiveEmail", user);
            SendEmail.Send(user.Email, "فعالسازی", body);

            await CreateAsync(user);
            return message;
        }

        public bool ActiveAccount(string activeCode)
        {
            var user = _context.Users.FirstOrDefault(x => x.ActiveCode == activeCode);
            if(user == null || user.IsActive)
                return false;

            user.IsActive = true;
            user.ActiveCode = NameGenerator.GenerateUniqCode();

            return true;
        }

        public async Task<bool> ResetPassword(ResetPasswordViewModel reset)
        {
            var user = _context.Users.FirstOrDefault(x => x.ActiveCode != reset.ActiveCode);
            if (user != null)
                return false;

            user.Password = reset.Password;
            UpdateAsync(user);

            return true;
        }

        public async Task<bool> ForgotPasswordService(ForgotPasswordViewModel reset)
        {
            var email = FixText.FixEmail(reset.Email);
            var user = _context.Users.FirstOrDefault(x => x.Email == email);

            if (user == null)
                return false;

            string bodyEmail = _viewRender.RenderToStringAsync("_ForgotPassword", user);
            SendEmail.Send(user.Email, "بازیابی کلمه عبور", bodyEmail);

            return true;

        }
    }
}
