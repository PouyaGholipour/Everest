using DomainLayer.Entities;
using DomainLayer.MainInterfaces;
using DomainServices.Exception;
using DomainServices.Interface;
using InfrastructureLayer.ApplicationDbContext;
using InfrastructureLayer.MainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainServices.Services
{
    public class CourseUserService : Repository<CourseUser>, ICourseUserService
    {
        private readonly EverestDataBaseContext _context;
        private readonly IUserRepositoy _userRepositoy;
        public IUnitOfWork _unitOfWork { get; }
        public CourseUserService(EverestDataBaseContext context,
                           IUnitOfWork unitOfWork,
                           IUserRepositoy userRepositoy) : base(context, unitOfWork)
        {
            this._context = (this._context ?? (EverestDataBaseContext)context);
            _userRepositoy = userRepositoy;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceException> AddCourseToUser(int userId, int courseId)
        {

            if (userId == 0 || courseId == 0)
                return ServiceException.Create(type: "NotFound",
                    title: "اطلاعات یافت نشد",
                    detail: ".اطلاعاتی با این شناسه ها برای انجام عملیات یافت نشد");

            var courseUser = new CourseUser()
            {
                CourseId = courseId,
                UserId = userId,
            };

            await CreateAsync(courseUser);
            return ServiceException.Create(type: "Success",
                    title: "عملیات موفق",
                    detail: "این دوره به دوره های شما افزوده شد.");
        }

        public async Task<ServiceException> RemoveCourseUser(int userId, int courseId)
        {
            if (userId == 0 || courseId == 0)
                return ServiceException.Create(type: "NotFound",
                    title: "اطلاعات یافت نشد",
                    detail: ".اطلاعاتی با این شناسه ها برای انجام عملیات یافت نشد");

            var userCourse = await GetListAsync(x => x.UserId == userId && x.CourseId == courseId);
            RemoveRange(userCourse);
            return ServiceException.Create(type: "Success",
                    title: "عملیات موفق",
                    detail: "این دوره از لیست دوره های شما حذف شد.");
        }
    }
}
