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
    public interface ICourseUserService : IRepository<CourseUser>
    {
        Task<ServiceException> AddCourseToUser(int userId, int courseId);
        Task<ServiceException> RemoveCourseUser(int userId, int courseId);
    }
}
