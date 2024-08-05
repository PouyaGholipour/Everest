using DomainServices.Exception;
using DomainServices.Interface;
using EverestAppUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EverestAppUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICourseService _courseService;
        private readonly IUserService _userService;
        private readonly IProgService _progService;

        public HomeController(ILogger<HomeController> logger,
                              IProgService progService,
                              ICourseService courseService,
                              IUserService userService)
        {
            _logger = logger;
            _userService = userService;
            _courseService = courseService;
            _progService = progService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Route("/Home/CourseDetail/{id?}")]
        public async Task<IActionResult> CourseDetail(int id)
        {
            if (id == 0)
            {
                var error = ServiceException.Create(
                    type: "NotFound",
                    title: "شناسه موجود نمیباشد.",
                    detail: "شناسه مورد نظر برای بارگذاری اطلاعات دوره یافت نشد.");
                ViewBag.error = error.Detail;

                return Redirect("/");
            }

            try
            {
                var courseDetails = await _courseService.GetCourseDetails(id);
                return View(courseDetails);
            }
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام بارگذاری اطلاعات دوره خطایی روی داد. لطفا دوباره تلاش کنید.");

                ViewBag.error = $"{exception.Title} {System.Environment.NewLine} {exception.Detail}";

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return Redirect("/Admin/Admin/GetPagedList");
            }
        }
    }
}