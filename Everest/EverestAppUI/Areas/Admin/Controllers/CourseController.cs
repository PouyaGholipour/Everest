using DomainLayer.DTOs.Course;
using DomainServices.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EverestAppUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetPagedList(int pageId = 1, string CourseTitleFilter = "")
        {
            var courseListModel = await _courseService.GetCourseList(pageId, CourseTitleFilter);
            return View(courseListModel);
        }

        public async Task<IActionResult> AddCourse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse(AddCourseViewModel courseViewModel)
        {
            if (!ModelState.IsValid)
                return View(courseViewModel);
            try
            {
                await _courseService.AddCourse(courseViewModel);
                return Redirect("/Admin/Course/GetPagedList");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "هنگام اضافه کردن دوره خطایی روی داد. لطفا دوباره تلاش کنید.");

                if (ex.InnerException != null)
                {
                    ModelState.AddModelError("", ex.InnerException.Message);
                }

                return View(courseViewModel);
            }
        }

        [HttpGet]
        [Route("/Admin/Course/EditCourse/{id?}")]
        public async Task<IActionResult> EditCourse(int courseId)
         {
            var editCourseViewModel = await _courseService.GetCourseForShowEditMode(courseId);
            return View(editCourseViewModel);
        }

        [HttpPost]
        [Route("/Admin/Course/EditCourse/{id?}")]
        public async Task<IActionResult> EditCourse(EditCourseViewModel courseViewModel)
        {
            try
            {
                await _courseService.EditCourseFromAdmin(courseViewModel);
                return Redirect("/Admin/Course/GetPagedList");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "هنگام ویرایش کردن دوره خطایی روی داد. لطفا دوباره تلاش کنید.");

                if (ex.InnerException != null)
                {
                    ModelState.AddModelError("", ex.InnerException.Message);
                }

                return View(courseViewModel);
            }
        }

        [Route("/Admin/Course/DeleteCourse/{id?}")]
        public IActionResult DeleteCourse(int courseId)
        {
            try
            {
                _courseService.DeleteCourse(courseId);
                return Redirect("/Admin/Course/GetPagedList");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "هنگام ویرایش کردن دوره خطایی روی داد. لطفا دوباره تلاش کنید.");

                if (ex.InnerException != null)
                {
                    ModelState.AddModelError("", ex.InnerException.Message);
                }

                return Redirect("/Admin/Course/GetPagedList");
            }
        }

    }
}
