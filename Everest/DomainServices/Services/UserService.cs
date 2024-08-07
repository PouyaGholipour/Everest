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
using DomainServices.Exception;
using DomainLayer.DTOs.Course;
using Microsoft.EntityFrameworkCore;

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

        public bool IsExistUserName(string userName) => _context.Users.Any(x => x.UserName == userName);

        public bool IsExistEmail(string email) =>  _context.Users.Any(x => x.Email == email);

        public async Task<ServiceException> LoginUser(LoginViewModel login)
        {                
            string email = FixText.FixEmail(login.Email);
            string password = login.Password;

            var user = _context.Users.FirstOrDefault(x => x.Email == email && x.Password == password);

            if (user == null)
                return ServiceException.Create(
                    type: "NotFound",
                    title: "موجودیت یافت نشد",
                    detail: "کاربر مورد نظر یافت نشد");

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
                return ServiceException.Create(
                    type: "Success",
                    title: "عملیات موفق",
                    detail: "ورود با موفقیت انجام شد.");
            }
            else
                return ServiceException.Create(
                    type: "NotActive",
                    title: "عملیات نا موفق",
                    detail: "حساب کاربری شما فعال نمی باشد.");
        }

        public void DeleteFromDataBase(int id)
        {
            // پیاده‌سازی حذف کاربر از پایگاه داده
            throw new NotImplementedException();
        }
        public async Task<ServiceException> AddUser(RegisterViewModel register)
        {
            // Register Dto is null
            if (register == null)
                return ServiceException.Create(
                type: "ValidationError",
                title: "اطلاعات یافت نشد",
                detail: "اطلاعات یافت نشد");
                
            // Email and Username is exist
            if (IsExistEmail(register.Email) && IsExistUserName(register.UserName))
                return ServiceException.Create(
                type: "ValidationError",
                title: "مشخضات",
                detail: "این نام کاربری و ایمیل قبلا استفاده شده است.");

            // Username is exist
            if (IsExistUserName(register.UserName))
                return ServiceException.Create(
                type: "ValidationError",
                title: "نام کاربری",
                detail: "این نام کاربری قبلا استفاده شده است.");

            // Email is exist
            if (IsExistEmail(register.Email))
                return ServiceException.Create(
                type: "ValidationError",
                title: "ایمیل",
                detail: "ایمیل وارد شده قبلا توسط کاربری استفاده شده است");

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
            return ServiceException.Create(
                type: "Success",
                title: "ایمیل",
                detail: "عملیات ثبت نام با موفقیت انجام شد");
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

        public ServiceException DeleteUser(int id)
        {
            var user = GetById(id);
            if (user == null)
                return ServiceException.Create(
                    type: "NotFound",
                    title: "موجودیت یافت نشد",
                    detail: "کاربری با این شناسه یافت نشد.");

            user.IsDelete = true;
            Update(user);
            return ServiceException.Create(
                    type: "Success",
                    title: "عملیات موفق",
                    detail: "عملیات حذف با موفقیت انجام شد");
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

        public async Task<ServiceException> EditUserFromAdmin(EditUserViewModel editUser)
        {
            User user = await _userRepositoy.GetUserWithRolesByIdAsync(editUser.UserId);

            if (user == null)
                return ServiceException.Create(type:"NotFound",
                    title:"کاربر یافت نشد",
                    detail:"کاربری بااین شناسه یافت نشد");

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
            return ServiceException.Create(type: "Found",
                    title: "کاربر با موفقیت ویرایش شد",
                    detail: "اطلاعات کاربر با موفقیت ویرایش شد");
        }

        public async Task<EditUserInformationViewModel> EditUserInformationGet(string userName)
        {
            var user = await GetAsync(u => u.UserName == userName);

            EditUserInformationViewModel editUser = new EditUserInformationViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                NationalCode = user.NationalCode,
                BirthdayDate = user.BirthDayDate,
                UserImage = user.ImageName
            };

            return editUser;
        }

        public async Task<ServiceException> EditUserInformationPost(EditUserInformationViewModel editUser, string name)
        {
            var user = await GetAsync(x => x.UserName == name);
            
            if(user == null)
                return ServiceException.Create(type: "NotFound",
                    title: "کاربر یافت نشد",
                    detail: "کاربری با این شناسه یافت نشد");

            user.RegisterDate = DateTime.Now;
            user.Email = editUser.Email;
            user.FirstName = editUser.FirstName;
            user.LastName = editUser.LastName;
            user.PhoneNumber = editUser.PhoneNumber;
            user.NationalCode = editUser.NationalCode;
            user.BirthDayDate = editUser.BirthdayDate;

            #region User Avatar

            if (editUser.ImageName != null)
            {
                // delete old image
                if (editUser.UserImage != "Default.jpg")
                {
                    string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", editUser.UserImage);
                    if (File.Exists(deletePath))
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
            return ServiceException.Create(type: "OK",
                    title: "اطلاعات با موفقیت ویرایش شد",
                    detail: "اطلاعات با موفقیت ویرایش شد");
        }

        public async Task<List<GetCourseForUserViewModel>> GetUserCourses(string userName)
        {
            return await _context.Users.Where(x => x.UserName == userName)
                .SelectMany(x => x.CourseUsers).Include(x => x.Course)
                .Select(x => new GetCourseForUserViewModel
                {
                    CourseId = x.CourseId,
                    CourseTitle = x.Course.CourseTitle,
                    Description = x.Course.Description,
                    HoldingDate = x.Course.DateOfHolding.ToString("yyyy-MM-dd")
                }).ToListAsync();
        }

        public async Task<List<GetProgForUserViewModel>> GetUserProgs(string userName)
        {
            return await _context.Users.Where(x => x.UserName == userName)
                .SelectMany(x => x.ProgUsers).Include(x => x.Prog)
                .Select(x => new GetProgForUserViewModel
                {
                    Id = x.Prog.Id,
                    Title = x.Prog.Title,
                    Description = x.Prog.Description,
                    DateOfHolding = x.Prog.DateOfHolding.ToString("yyyy-MM-dd")
                }).ToListAsync();
        }
    }
}
