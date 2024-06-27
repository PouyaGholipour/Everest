using DomainLayer.DTOs.User;
using DomainLayer.Entities;
using DomainLayer.MainInterfaces;
using DomainLayer.ServiceResults;
using DomainServices.Interface;
using DomainServices.Services;
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

        public IActionResult LoginPage()
        {
            return View();
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

            string message = await _userService.AddUser(register);

            switch (message)
            {
                case "UsernameExist":
                    ModelState.AddModelError("UserName", "این نام کاربری قبلا استفاده شده است");
                    return View(register);
                    break;

                case "EmailExist":
                    ModelState.AddModelError("Email", "ایمیل وارد شده قبلا استفاده شده است");
                    return View(register);
                    break;
                
                case "UsernameEmailExist":
                    ModelState.AddModelError("Email", "ایمیل وارد شده قبلا استفاده شده است");
                    ModelState.AddModelError("UserName", "این نام کاربری قبلا استفاده شده است");
                    return View(register);
                    break;
            }

            ViewBag.IsSuccess = true;

            return View(register);
        }
    }
}
