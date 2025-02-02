﻿using DomainLayer.DTOs.Course;
using DomainServices.Exception;
using DomainServices.Interface;
using DomainServices.Security;
using Microsoft.AspNetCore.Mvc;

namespace EverestAppUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker(6)]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }
        [PermissionChecker(6)]
        public IActionResult Index()
        {
            return View();
        }

        [PermissionChecker(6)]
        public async Task<IActionResult> GetPagedList(int pageId = 1, string CourseTitleFilter = "")
        {
            try
            {
                var courseListModel = await _courseService.GetCourseList(pageId, CourseTitleFilter);
                return View(courseListModel);
            }
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام بارگذاری لیست دوره ها خطایی روی داد. لطفا دوباره تلاش کنید.");

                ViewBag.error = exception.Detail;

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return Redirect("/Admin/Admin/Index/");
            }
        }

        [PermissionChecker(7)]
        public IActionResult AddCourse()
        {
            return View();
        }

        [PermissionChecker(7)]
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
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام اضافه کردن دوره جدید خطایی روی داد. لطفا دوباره تلاش کنید.");

                ViewBag.error = $"{exception.Detail}";

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return View(courseViewModel);
            }
        }

        [PermissionChecker(8)]
        [HttpGet]
        [Route("/Admin/Course/EditCourse/{id?}")]
        public async Task<IActionResult> EditCourse(int courseId)
         {
            if(courseId == 0)
            {
                var exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام بارگذاری اطلاعات دوره خطایی روی داد. لطفا دوباره تلاش کنید.");
                ViewBag.error = $"{exception.Detail}";
                return Redirect("/Admin/Course/GetPagedList");
            }
            var editCourseViewModel = await _courseService.GetCourseForShowEditMode(courseId);
            return View(editCourseViewModel);
        }

        [PermissionChecker(8)]
        [HttpPost]
        [Route("/Admin/Course/EditCourse/{id?}")]
        public async Task<IActionResult> EditCourse(EditCourseViewModel courseViewModel)
        {
            try
            {
                var result = await _courseService.EditCourseFromAdmin(courseViewModel);

                if(result.Type =="Success")
                    ViewBag.result = $"{result.Detail}";
                else
                    ViewBag.error = $"{result.Detail}";

                return Redirect("/Admin/Course/GetPagedList");
            }
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام ویرایش دوره خطایی روی داد. لطفا دوباره تلاش کنید.");

                ViewBag.error = exception.Detail;

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return View(courseViewModel);
            }
        }

        [PermissionChecker(9)]
        [Route("/Admin/Course/DeleteCourse/{id?}")]
        public IActionResult DeleteCourse(int courseId)
        {
            try
            {
                var result = _courseService.DeleteCourse(courseId);

                if (result.Type == "NotFound")
                    ViewBag.error = $"{result.Detail}";

                return Redirect("/Admin/Course/GetPagedList");
            }
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام حذف دوره خطایی روی داد. لطفا دوباره تلاش کنید.");
                ViewBag.error = $"{exception.Detail}";

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return Redirect("/Admin/Course/GetPagedList");
            }
        }

    }
}
