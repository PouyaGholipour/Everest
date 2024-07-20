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
        #region Field
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Constructor
        public UserController(IUserService userService, IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region CRUD Operation

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

            if(result.Type == "Success")
                ViewBag.IsSuccess = true;

            else
                ViewData["message"] = $"{result.Detail}";

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

            var message = await _userService.AddUser(register);

            if(message.Type == "Success")
                ViewBag.IsSuccess = true;

            else
            {
                ViewData["message"] = $"{message.Detail}";
                return View(register);
            }

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

        #endregion
    }
}
