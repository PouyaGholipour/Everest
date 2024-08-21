using DomainLayer.DTOs.Report;
using DomainServices.Exception;
using DomainServices.Interface;
using DomainServices.Security;
using Microsoft.AspNetCore.Mvc;

namespace EverestAppUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker(14)]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        [PermissionChecker(14)]
        public IActionResult Index()
        {
            return View();
        }

        [PermissionChecker(14)]
        public IActionResult GetPagedList(int pageId = 1, string reportTitleFilter = "")
        {
            try
            {
                var viewModel = _reportService.GetPagedList(pageId, reportTitleFilter);
                return View(viewModel);
            }
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام بارگذاری گزارشات خطایی روی داد. لطفا دوباره تلاش کنید.");
                ViewBag.error = $"{exception.Detail}";

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return Redirect("/Admin/Admin/Index/");
            }
        }

        [PermissionChecker(15)]
        [HttpGet]
        public IActionResult AddReport()
        {
            return View();
        }

        [PermissionChecker(15)]
        [HttpPost]
        public async Task<IActionResult> AddReport(AddReportViewModel addReportViewModel)
        {
            if (!ModelState.IsValid)
                return View(addReportViewModel);
            try
            {
                await _reportService.AddReport(addReportViewModel);
                return Redirect("/Admin/Report/GetPagedList/");
            }
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام اضافه کردن گزارش خطایی روی داد. لطفا دوباره تلاش کنید.");
                ViewBag.error = $"{exception.Detail}";

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return Redirect("/Admin/Report/GetPagedList/");
            }
        }

        [PermissionChecker(16)]
        [HttpGet]
        [Route("/Admin/Report/EditReport/{id?}")]
        public async Task<IActionResult> EditReport(int reportId)
        {
            if(reportId == 0)
            {
                var exception = ServiceException.Create(
                    type: "NotFound",
                    title: "موجودیت یافت نشد.",
                    detail: "گزارشی با این شناسه موجودیت یافت نشد.");
                ViewBag.error = exception.Detail;

                return Redirect("/Admin/Prog/GetPagedList/");
            }
            var viewModel = await _reportService.GetReportForShowEditMode(reportId);
            return View(viewModel);
        }

        [PermissionChecker(16)]
        [HttpPost]
        [Route("/Admin/Report/EditReport/{id?}")]
        public async Task<IActionResult> EditReport(EditReportViewModel editReportViewModel)
        {
            try
            {
                await _reportService.EditReport(editReportViewModel);
                return Redirect("/Admin/Report/GetPagedList/");
            }
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "عملیات ناموفق",
                    detail: "هنگام ویرایش گزارش خطایی روی داد. لطفا دوباره تلاش کنید.");
                ViewBag.error = $"{exception.Detail}";

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return Redirect("/Admin/Report/GetPagedList/");
            }
        }
        [PermissionChecker(17)]
        [Route("/Admin/Report/RemoveReport/{id?}")]
        public IActionResult RemoveReport(int reportId)
        {
            try
            {
                var result = _reportService.RemoveReport(reportId);
                if(result.Type == "NotFound")
                    ViewBag.error = $"{result.Detail}";

                return Redirect("/Admin/Report/GetPagedList/");
            }
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "عملیات ناموفق",
                    detail: "هنگام حذف گزارش خطایی روی داد. لطفا دوباره تلاش کنید.");

                ViewBag.error = $"{exception.Detail}";

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return Redirect("/Admin/Report/GetPagedList/");
            }
        }
    }
}
