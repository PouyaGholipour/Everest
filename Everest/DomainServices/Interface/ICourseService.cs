using DomainLayer.DTOs.Course;
using DomainLayer.DTOs.User;
using DomainLayer.Entities;
using DomainLayer.MainInterfaces;
using DomainServices.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainServices.Interface
{
    public interface ICourseService : IRepository<Course>
    {
        // Definition private function model

        Task<CourseListViewModel> GetCourseList(int pageId = 1, string courseTitleFilter = "");
        Task<List<HeldCourseListViewModel>> GetHeldCourseList(int take);
        Task AddCourse(AddCourseViewModel addCourse);
        Task<EditCourseViewModel> GetCourseForShowEditMode(int courseId);
        Task<ServiceException> EditCourseFromAdmin(EditCourseViewModel editCourse);
        ServiceException DeleteCourse(int courseId);
        Task<CourseDetailViewModel> GetCourseDetails(int id); 
    }
}
