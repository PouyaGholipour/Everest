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
        private readonly ICourseUserService _courseUserService;
        private readonly IProgUserService _progUserService;
        public HomeController(IUserService userService,
                              ICourseUserService courseUserService,
                              IProgUserService progUserService)
        {
            _userService = userService;
            _courseUserService = courseUserService;
            _progUserService = progUserService;
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
            try
            {
                var courseList = await _userService.GetUserCourses(User.Identity.Name);
                return View(courseList);
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
        public async Task<IActionResult> GetUserProgs()
        {
            try
            {
                var progList = await _userService.GetUserProgs(User.Identity.Name);
                return View(progList);
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
        public async Task<IActionResult> RemoveCourseUser(int CourseId)
        {
            try
            {
                var user = await _userService.GetAsync(x => x.UserName == User.Identity.Name);
                var result = await _courseUserService.RemoveCourseUser(user.Id, CourseId);

                if (result.Type == "NotFound")
                    ViewBag.error = $"{result.Detail}";
                else if (result.Type == "Success")
                    ViewBag.Message = $"{result.Detail}";

                return View();
            }
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام انجام عملیات خطایی روی داد. لطفا دوباره تلاش کنید.");

                ViewBag.error = $"{exception.Title} {System.Environment.NewLine} {exception.Detail}";

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return Redirect("/");
            }
        }
        [HttpGet]
        public async Task<IActionResult> RemoveProgUser(int progId)
        {
            try
            {
                var user = await _userService.GetAsync(x => x.UserName == User.Identity.Name);
                var result = await _progUserService.RemoveProgUser(user.Id, progId);

                if (result.Type == "NotFound")
                    ViewBag.error = $"{result.Detail}";
                else if (result.Type == "Success")
                    ViewBag.Message = $"{result.Detail}";

                return View();
            }
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام انجام عملیات خطایی روی داد. لطفا دوباره تلاش کنید.");

                ViewBag.error = $"{exception.Title} {System.Environment.NewLine} {exception.Detail}";

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return Redirect("/");
            }
        }
    }
}
