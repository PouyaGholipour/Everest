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
using Microsoft.AspNetCore.Mvc;

namespace DomainServices.Services
{
    public class UserService : Repository<User>, IUserService
    {
        private readonly EverestDataBaseContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IViewRenderService _viewRender;
        private readonly IUserRepositoy _userRepositoy;
        public IUnitOfWork _unitOfWork { get; }
        public UserService(EverestDataBaseContext context,
                           IUnitOfWork unitOfWork,
                           IHttpContextAccessor httpContextAccessor,
                           IUserRepositoy userRepositoy,
                           IViewRenderService viewRender) : base(context, unitOfWork)
        {
            this._context = (this._context ?? (EverestDataBaseContext)context);
            _httpContextAccessor = httpContextAccessor;
            _viewRender = viewRender;
            _userRepositoy = userRepositoy;
            _unitOfWork = unitOfWork;
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
            await SendEmail.Send(user.Email, "فعالسازی", body);

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
            await UpdateAsync(user);

            return true;
        }

        public async Task<bool> ForgotPasswordService(ForgotPasswordViewModel reset)
        {
            var email = FixText.FixEmail(reset.Email);
            var user = _context.Users.FirstOrDefault(x => x.Email == email);

            if (user == null)
                return false;

            string bodyEmail = _viewRender.RenderToStringAsync("_ForgotPassword", user);
            await SendEmail.Send(user.Email, "بازیابی کلمه عبور", bodyEmail);

            return true;

        }

        public async Task<UserListViewModel> GetUserList(int pageId = 1, string userNameFilter = "", string emailFilter = "")
        {
            IQueryable<User> result = _context.Users;

            if (!string.IsNullOrEmpty(userNameFilter))
                result = result.Where(x => x.UserName.Contains(userNameFilter));

            if (!string.IsNullOrEmpty(emailFilter))
                result = result.Where(x => x.Email.Contains(emailFilter));

            double take = 10;
            double skip = (pageId - 1) * take;

            UserListViewModel userList = new UserListViewModel();
            userList.CurrentPage = pageId;
            var pageCount = (Math.Ceiling(result.Count() / take));
            userList.PageCount = Convert.ToInt32(pageCount);
            userList.Users = result.OrderBy(x => x.RegisterDate).Skip(Convert.ToInt32(skip)).Take(Convert.ToInt32(take)).ToList();

            return userList;
        }
        
        public async Task<EditUserViewModel> GetUserForShowEditMode(int id)
        {
            var user = await _userRepositoy.GetUserWithRolesByIdAsync(id);

            var viewModel = new EditUserViewModel()
            {
                UserId = user.Id,
                Email = user.Email,
                AvatarName = user.ImageName,
                UserName = user.UserName,
                Password = user.Password,
                UserRoles = user.RoleUsers.Select(x => x.RoleId).ToList(),
            };
            return viewModel;
        }

        public void DeleteUser(int id)
        {
            var user = GetById(id);
            user.IsDelete = true;
            Update(user);
        }

        public async Task<int> CreateUserFromAdmin(CreateUserViewModel createUser, List<int> SelectedRoles)
        {
            
            User user = new User();
            user.Email = createUser.Email;
            user.UserName = createUser.UserName;
            user.Password = createUser.Password;
            user.IsActive = true;
            user.ActiveCode = NameGenerator.GenerateUniqCode();
            user.RegisterDate = DateTime.Now;


            if(SelectedRoles.Contains(1))
                user.UserType = UserType.User;

            if (SelectedRoles.Contains(2))
                user.UserType = UserType.SpecialUser;

            if (SelectedRoles.Contains(3))
                user.UserType = UserType.AdminUser;

            if (SelectedRoles.Contains(4))
                user.UserType = UserType.Manager;

            #region Save Avatar

            if (createUser.ImageName != null)
            {
                string imagePath = "";
                user.ImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(createUser.ImageName.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", user.ImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    createUser.ImageName.CopyTo(stream);
                }
                user.ImagePath = imagePath;
            }
            
            #endregion
            
            return AddUser(user);
        }

        public int AddUser(User user)
        {
            Create(user);
            _unitOfWork.Commit();
            return user.Id;
        }

        public async Task EditUserFromAdmin(EditUserViewModel editUser)
        {
            User user = await _userRepositoy.GetUserWithRolesByIdAsync(editUser.UserId);
            user.Email = editUser.Email;
            
            if(string.IsNullOrEmpty(editUser.Password))
                user.Password = editUser.Password;
            #region User Avatar

            if (editUser.ImageName != null)
            {
                // delete old image
                if(editUser.AvatarName != "Default.jpg")
                {
                    string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", editUser.AvatarName);
                    if(File.Exists(deletePath))
                        File.Delete(deletePath);
                }

                // Save new image
                user.ImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(editUser.ImageName.FileName);
                 var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", user.ImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    editUser.ImageName.CopyTo(stream);
                }
                user.ImagePath = imagePath;
            }
            
            #endregion

            await UpdateAsync(user);
        }
    }
}
