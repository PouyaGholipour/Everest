using DomainLayer.DTOs.Course;
using DomainServices.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EverestAppUI.ViewComponents
{
    public class CourseListComponent : ViewComponent
    {
        private ICourseService _courseService;
        private readonly ILogger<CourseListComponent> _logger;
        public CourseListComponent(ICourseService courseService, ILogger<CourseListComponent> logger)
        {
            _courseService = courseService;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var courseList = await _courseService.GetHeldCourseList(6);
                return View("CourseList", courseList);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CourseListComponent: {ex.Message}");
                return View(new List<HeldCourseListViewModel>());
            }
        }
    }
}
