using DomainLayer.DTOs.Journal;
using DomainServices.Exception;
using DomainServices.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EverestAppUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class JournalController : Controller
    {
        private readonly IJournalService _journalService;
        public JournalController(IJournalService journalService)
        {
            _journalService = journalService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetPagedList(int pageId = 1, string journalTitleFilter = "")
        {
            try
            {
                var viewModel = _journalService.GetPagedList(pageId, journalTitleFilter);
                return View(viewModel);
            }
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام بارگذاری لیست مجله ها خطایی روی داد. لطفا دوباره تلاش کنید.");

                ViewBag.error = exception.Detail;

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return Redirect("/Admin/Admin/Index/");
            }
        }

        [HttpGet]
        public IActionResult AddJournal()
        {
            return View();
        }

        [HttpPost]
        [Route("/Admin/Journal/AddJournal/")]
        public async Task<IActionResult> AddJournal(AddJournalViewModel addJournalViewModel)
        {
            if (!ModelState.IsValid)
                return View(addJournalViewModel);
            try
            {
                await _journalService.AddJournal(addJournalViewModel);
                return Redirect("/Admin/Journal/GetPagedList/");
            }
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام افزودن مجله خطایی روی داد. لطفا دوباره تلاش کنید.");

                ViewBag.error = exception.Detail;

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return Redirect("/Admin/Admin/Index/");
            }
        }
        [HttpGet]
        [Route("/Admin/Journal/EditJournal/{id?}")]
        public async Task<IActionResult> EditJournal(int journalId)
        {
            var JournalViewModel = await _journalService.GetJournalForShowEditMode(journalId);
            return View(JournalViewModel);
        }

        [HttpPost]
        [Route("/Admin/Journal/EditJournal/{id?}")]
        public async Task<IActionResult> EditJournal(EditJournalViewModel editJournalViewModel)
        {
            try
            {
                await _journalService.EditJournal(editJournalViewModel);
                return Redirect("/Admin/Journal/GetPagedList/");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "هنگام ویرایش مجله ها خطایی روی داد. لطفا دوباره تلاش کنید.");

                if (ex.InnerException != null)
                {
                    ModelState.AddModelError("", ex.InnerException.Message);
                }

                return View(editJournalViewModel);
            }
        }

        [Route("/Admin/Journal/DeleteJournal/")]
        public IActionResult DeleteJournal(int journalId)
        {
            try
            {
                _journalService.RemoveJournal(journalId);
                return Redirect("/Admin/journal/GetPagedList/");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "هنگام ویرایش مجله ها خطایی روی داد. لطفا دوباره تلاش کنید.");

                if (ex.InnerException != null)
                {
                    ModelState.AddModelError("", ex.InnerException.Message);
                }

                return Redirect("/Admin/Journal/GetPagedList/");
            }
        }
    }
}
