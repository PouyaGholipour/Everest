using DomainLayer.DTOs.User;
using DomainServices.Exception;
using DomainServices.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EverestAppUI.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        public HomeController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditUserInformation()
        {
            try
            {
                var userInformation = _userService.EditUserInformationGet(User.Identity.Name).Result;
                return View(userInformation);
            }
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام بارگذاری اطلاعات خطایی روی داد. لطفا دوباره تلاش کنید.");

                ViewBag.error = $"{exception.Title} {System.Environment.NewLine} {exception.Detail}";

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return Redirect("/User/Home/Index/");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditUserInformation(EditUserInformationViewModel editUser)
        {
            try
            {
                var resultMessage = await _userService.EditUserInformationPost(editUser, User.Identity.Name);
                if (resultMessage.Type == "OK")
                    ViewBag.IsSuccess = $"{resultMessage.Detail}";

                else if (resultMessage.Type == "NotFound")
                    ViewBag.error = $"{resultMessage.Detail}";

                return View(editUser);
            }
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام بارگذاری اطلاعات خطایی روی داد. لطفا دوباره تلاش کنید.");

                ViewBag.error = $"{exception.Title} {System.Environment.NewLine} {exception.Detail}";

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return Redirect("/User/Home/Index/");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUserCourses()
        {

            return View();
        }
    }
}
