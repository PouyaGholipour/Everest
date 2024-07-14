using DomainLayer.DTOs.Prog;
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
            catch (Exception ex)
            {
                ModelState.AddModelError("", "هنگام بارگذاری برنامه ها خطایی روی داد. لطفا دوباره تلاش کنید.");

                if (ex.InnerException != null)
                {
                    ModelState.AddModelError("", ex.InnerException.Message);
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
                if (!ModelState.IsValid)
                    return View(addProgViewModel);

                await _progService.AddProg(addProgViewModel);
                return Redirect("/Admin/Prog/GetPagedList/");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "هنگام اضافه کردن برنامه ها خطایی روی داد. لطفا دوباره تلاش کنید.");

                if (ex.InnerException != null)
                {
                    ModelState.AddModelError("", ex.InnerException.Message);
                }

                return View(addProgViewModel);
            }
        }
        [HttpGet]
        [Route("/Admin/Prog/EditProg/{id?}")]
        public async Task<IActionResult> EditProg(int progId)
        {
            var progViewModel = await _progService.GetProgForShowEditMode(progId);
            return View(progViewModel);
        }

        [HttpPost]
        [Route("/Admin/Prog/EditProg/{id?}")]
        public async Task<IActionResult> EditProg(EditProgViewModel editProgViewModel)
        {
            try
            {
                await _progService.EditProg(editProgViewModel);
                return Redirect("/Admin/Prog/GetPagedList/");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "هنگام ویرایش برنامه ها خطایی روی داد. لطفا دوباره تلاش کنید.");

                if (ex.InnerException != null)
                {
                    ModelState.AddModelError("", ex.InnerException.Message);
                }

                return View(editProgViewModel);
            }
        }

        [Route("/Admin/Prog/DeleteProg/")]
        public IActionResult DeleteProg(int progId)
        {
            try
            {
                _progService.RemoveProg(progId);
                return Redirect("/Admin/Prog/GetPagedList/");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "هنگام ویرایش برنامه ها خطایی روی داد. لطفا دوباره تلاش کنید.");

                if (ex.InnerException != null)
                {
                    ModelState.AddModelError("", ex.InnerException.Message);
                }

                return Redirect("/Admin/Prog/GetPagedList/");
            }
        }
    }
}
