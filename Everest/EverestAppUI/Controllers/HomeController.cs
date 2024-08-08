using DomainLayer.DTOs.CourseUser;
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
        private readonly IJournalService _journalService;
        private readonly IReportService _reportlService;
        private readonly ICourseUserService _courseUserService;
        private readonly IProgUserService _progUserService;

        public HomeController(ILogger<HomeController> logger,
                              IProgService progService,
                              ICourseService courseService,
                              IUserService userService,
                              ICourseUserService courseUserService,
                              IProgUserService progUserService,
                              IJournalService journalService,
                              IReportService reportService)
        {
            _logger = logger;
            _userService = userService;
            _courseService = courseService;
            _progService = progService;
            _courseUserService = courseUserService;
            _progUserService = progUserService;
            _journalService = journalService;
            _reportlService = reportService;
        }

        public IActionResult Index()
        {
            var user = _userService.GetAsync(x => x.UserName == User.Identity.Name).Result;
            ViewBag.Username = User.Identity.Name;
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

        #region Course

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

                return Redirect("/");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ShowMoreCourse()
        {
            try
            {
                var courseList = await _courseService.GetHeldCourseList(20);
                return View(courseList);
            }
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام بارگذاری لیست دوره ها خطایی روی داد. لطفا دوباره تلاش کنید.");

                ViewBag.error = $"{exception.Title} {System.Environment.NewLine} {exception.Detail}";

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return Redirect("/");
            }
        }

        #endregion
        #region Prog

        [HttpGet]
        [Route("/Home/ProgDetail/{id?}")]
        public async Task<IActionResult> ProgDetail(int id)
        {
            if (id == 0)
            {
                var error = ServiceException.Create(
                    type: "NotFound",
                    title: "شناسه موجود نمیباشد.",
                    detail: "شناسه مورد نظر برای بارگذاری اطلاعات برنامه یافت نشد.");
                ViewBag.error = error.Detail;

                return Redirect("/");
            }
            try
            {
                var progDetails = await _progService.GetProgDetail(id);
                return View(progDetails);
            }
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام بارگذاری اطلاعات برنامه خطایی روی داد. لطفا دوباره تلاش کنید.");

                ViewBag.error = $"{exception.Title} {System.Environment.NewLine} {exception.Detail}";

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return Redirect("/");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ShowMoreProgs()
        {
            try
            {
                var progList = await _progService.GetHeldProgList(20);
                return View(progList);
            }
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام بارگذاری لیست برنامه ها خطایی روی داد. لطفا دوباره تلاش کنید.");

                ViewBag.error = $"{exception.Title} {System.Environment.NewLine} {exception.Detail}";

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return Redirect("/");
            }
        }
        #endregion

        #region CourseUser

        [HttpGet]
        public async Task<IActionResult> AddCourseToUser(int CourseId)
        {
            try
            {
                var user = await _userService.GetAsync(x => x.UserName == User.Identity.Name);
                var result = await _courseUserService.AddCourseToUser(user.Id, CourseId);

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


        #endregion

        #region ProgUser

        [HttpGet]
        public async Task<IActionResult> AddProgToUser(int progId)
        {
            try
            {
                var user = await _userService.GetAsync(x => x.UserName == User.Identity.Name);
                var result = await _progUserService.AddProgToUser(user.Id, progId);

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

        #endregion

        #region Journal

        [HttpGet]
        public async Task<IActionResult> ShowJournalList()
        {
            try
            {
                var journalList = await _journalService.GetJournaList(20);
                return View(journalList);
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
        public async Task<IActionResult> JournalDetail(int journalId)
        {
            if (journalId == 0)
            {
                var error = ServiceException.Create(
                    type: "NotFound",
                    title: "شناسه موجود نمیباشد.",
                    detail: "شناسه مورد نظر برای بارگذاری اطلاعات مجله یافت نشد.");
                ViewBag.error = error.Detail;

                return Redirect("/");
            }
            try
            {
                var journalDetail = await _journalService.GetJournalDetail(journalId);
                return View(journalDetail);
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

        #endregion

        #region Report

        [HttpGet]
        public async Task<IActionResult> GetReportList()
        {
            try
            {
                var reportList = await _reportlService.GetReportList(20);
                return View(reportList);
            }
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام انجام عملیات خطایی روی داد. لطفا دوباره تلاش کنید.");

                ViewBag.error = $"{exception.Detail}";

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return Redirect("/");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ReportDetail(int reportId)
        {
            if (reportId == 0)
            {
                var error = ServiceException.Create(
                    type: "NotFound",
                    title: "شناسه موجود نمیباشد.",
                    detail: "شناسه مورد نظر برای بارگذاری اطلاعات گزارش یافت نشد.");
                ViewBag.error = error.Detail;

                return Redirect("/");
            }
            try
            {
                var reportDetail = await _reportlService.GetReportlDetail(reportId);
                return View(reportDetail);
            }
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام انجام عملیات خطایی روی داد. لطفا دوباره تلاش کنید.");

                ViewBag.error = $"{exception.Detail}";

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return Redirect("/");
            }
        } 

        #endregion
    }
}