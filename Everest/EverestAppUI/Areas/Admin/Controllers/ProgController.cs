using DomainLayer.DTOs.Prog;
using DomainServices.Exception;
using DomainServices.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EverestAppUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProgController : Controller
    {
        private readonly IProgService _progService;
        public ProgController(IProgService progService)
        {
            _progService = progService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetPagedList(int pageId = 1, string progTitleFilter = "")
        {
            try
            {
                var viewModel = _progService.GetPagedList(pageId, progTitleFilter);
                return View(viewModel);
            }
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام بارگذاری لیست برنامه ها خطایی روی داد. لطفا دوباره تلاش کنید.");
                ViewBag.error = $"{exception.Detail}";

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return Redirect("/Admin/Admin/Index/");
            }
        }

        [HttpGet]
        public IActionResult AddProg()
        {
            return View();
        }

        [HttpPost]
        [Route("/Admin/Prog/AddProg/")]
        public async Task<IActionResult> AddProg(AddProgViewModel addProgViewModel)
        {
            if (!ModelState.IsValid)
                return View(addProgViewModel);
            try
            {
                await _progService.AddProg(addProgViewModel);
                return Redirect("/Admin/Prog/GetPagedList/");
            }
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام اضافه کردن برنامه جدید خطایی روی داد. لطفا دوباره تلاش کنید.");
                ViewBag.error = $"{exception.Detail}";

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return View(addProgViewModel);
            }
        }
        [HttpGet]
        [Route("/Admin/Prog/EditProg/{id?}")]
        public async Task<IActionResult> EditProg(int progId)
        {
            if(progId == 0)
            {
                var exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام بارگذاری اظلاعات برنامه خطایی روی داد. لطفا دوباره تلاش کنید.");

                ViewBag.error = $"{exception.Detail}";

                return Redirect("/Admin/Prog/GetPagedList/");
            }
            var progViewModel = await _progService.GetProgForShowEditMode(progId);
            return View(progViewModel);
        }

        [HttpPost]
        [Route("/Admin/Prog/EditProg/{id?}")]
        public async Task<IActionResult> EditProg(EditProgViewModel editProgViewModel)
        {
            try
            {
                var result = await _progService.EditProg(editProgViewModel);
                if (result.Type == "NotFound")
                    ViewBag.error = $"{result.Detail}";

                return Redirect("/Admin/Prog/GetPagedList/");
            }
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام ویرایش برنامه خطایی روی داد. لطفا دوباره تلاش کنید.");

                ViewBag.error = $"{exception.Detail}";

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return View(editProgViewModel);
            }
        }

        [Route("/Admin/Prog/DeleteProg/")]
        public IActionResult DeleteProg(int progId)
        {
            try
            {
                var result = _progService.RemoveProg(progId);
                if (result.Type == "NotFound")
                    ViewBag.error = $"{result.Detail}";
                return Redirect("/Admin/Prog/GetPagedList/");
            }
            catch (Exception ex)
            {
                ViewBag.error = "هنگام حذف برنامه خطایی روی داد. لطفا دوباره تلاش کنید.";

                if (ex.InnerException != null)
                {
                    ViewBag.error += "" + ex.InnerException.Message;
                }

                return Redirect("/Admin/Prog/GetPagedList/");
            }
        }
    }
}
