using DomainLayer.DTOs.User;
using DomainLayer.Entities;
using DomainLayer.Enums;
using DomainLayer.MainInterfaces;
using DomainLayer.ServiceResults;
using DomainServices.Interface;
using DomainServices.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace EverestAppUI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IUserService userService, IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Login")]
        public IActionResult LoginPage()
        {
            return View();
        }
        
        [HttpPost]
        [Route("/Login")]
        public async Task<IActionResult> LoginPage(LoginViewModel login)
        {
            if(!ModelState.IsValid)
                return View(login);

            var result = await _userService.LoginUser(login);

            switch(result)
            {
                case ClientMessageType.Null:
                    ModelState.AddModelError("Email", "کاربری با این ایمیل یافت نشد!");
                    return View(login);
                    break;
                case ClientMessageType.IsNotActive:
                    ViewBag.IsActive = false;
                    break;
                case ClientMessageType.Success:
                    ViewBag.IsSuccess = true;
                    break;
            }

            return View(login);
        }

        [Route("/Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel register)
         {
            if (!ModelState.IsValid)
                return View(register);

            ClientMessageType message = await _userService.AddUser(register);

            switch (message)
            {
                case ClientMessageType.UsernameIsExist:
                    ModelState.AddModelError("UserName", "این نام کاربری قبلا استفاده شده است");
                    return View(register);
                    break;

                case ClientMessageType.EmailIsExist:
                    ModelState.AddModelError("Email", "ایمیل وارد شده قبلا استفاده شده است");
                    return View(register);
                    break;
                
                case ClientMessageType.UsernameEmaliIsExist:
                    ModelState.AddModelError("Email", "ایمیل وارد شده قبلا استفاده شده است");
                    ModelState.AddModelError("UserName", "این نام کاربری قبلا استفاده شده است");
                    return View(register);
                    break;
            }

            ViewBag.IsSuccess = true;

            return View("SuccessRegister", register);
        }

        [HttpPost]
        public IActionResult ActiveAccount(string activeCode)
        {
            ViewBag.IsActive = _userService.ActiveAccount(activeCode);
            return View();
        }

        [Route("/ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        
        [HttpPost]
        [Route("/ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgot)
        {
            if (!ModelState.IsValid)
                return View(forgot);

            var result = await _userService.ForgotPasswordService(forgot);

            if (!result)
                return View(forgot);
            else
                ViewBag.IsSuccess = result;
                return View();
        }

        [Route("/ResetPassword")]
        public IActionResult ResetPassword(string id)
        {
            return View(new ResetPasswordViewModel
            {
                ActiveCode = id
            });
        }

        [HttpPost]
        [Route("/ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel reset)
        {
            if (!ModelState.IsValid)
                return View(reset);

            bool result = await _userService.ResetPassword(reset);

            if (!result)
                return NotFound();
            else
                return RedirectToAction("LoginPage", "User");
        }
    }
}
