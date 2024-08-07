using DomainLayer.DTOs.Course;
using DomainLayer.DTOs.User;
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
using TopLearn.Core.Generators;

namespace DomainServices.Services
{
    public class CourseService : Repository<Course>, ICourseService
    {
        private readonly EverestDataBaseContext _context;
        public IUnitOfWork _unitOfWork { get; }
        public CourseService(EverestDataBaseContext context
                            , IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = (this._context ?? (EverestDataBaseContext)context);
            _unitOfWork = unitOfWork;
        }

        public async Task<CourseListViewModel> GetCourseList(int pageId = 1, string courseTitleFilter = "")
        {
            IQueryable<Course> result = _context.Courses;

            if (!string.IsNullOrEmpty(courseTitleFilter))
                result = result.Where(x => x.CourseTitle.Contains(courseTitleFilter));

            double take = 10;
            double skip = (pageId - 1) * take;

            CourseListViewModel CoursesList = new CourseListViewModel();
            CoursesList.CurrentPage = pageId;
            var pageCount = (Math.Ceiling(result.Count() / take));
            CoursesList.PageCount = Convert.ToInt32(pageCount);
            CoursesList.Courses = result.OrderBy(x => x.DateOfHolding).Skip(Convert.ToInt32(skip)).Take(Convert.ToInt32(take)).ToList();

            return CoursesList;
        }

        public async Task AddCourse(AddCourseViewModel addCourse)
        {
            Course course = new Course();
            course.CourseTitle = addCourse.CourseTitle;
            course.CoachName = addCourse.CoachName; 
            course.Description = addCourse.Description;
            course.Place = addCourse.Place;
            course.Pirce = Convert.ToDecimal(addCourse.Pirce);
            course.CourseType = addCourse.CourseType;
            course.DateOfHolding = DateTime.Parse(addCourse.DateOfHolding);
            course.CategoryId = 1;
            course.Status = addCourse.Status;
            course.WhichCoursePrerequisites = addCourse.WhichCoursePrerequisites;
            course.PrerequisiteCourse = addCourse.PrerequisiteCourse;

            #region Save Avatar

            if (addCourse.ImageName != null)
            {
                string imagePath = "";
                course.ImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(addCourse.ImageName.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseImage", course.ImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    addCourse.ImageName.CopyTo(stream);
                }
                course.ImagePath = imagePath;
            }

            #endregion
            await CreateAsync(course);
            await _unitOfWork.CommitAsync();
        }

        public async Task<EditCourseViewModel> GetCourseForShowEditMode(int courseId)
        {
            var course = GetAsync(c => c.Id == courseId).Result;
            EditCourseViewModel viewModel = new EditCourseViewModel();
            viewModel.CourseTitle = course.CourseTitle;
            viewModel.Description = course.Description;
            viewModel.DateOfHolding = course.DateOfHolding.ToString("yyyy-MM-dd");
            viewModel.CourseImage = course.ImageName;
            viewModel.CourseType = course.CourseType;
            viewModel.ImagePath = course.ImagePath;
            viewModel.Pirce = course.Pirce.ToString();
            viewModel.PrerequisiteCourse = course.PrerequisiteCourse;
            viewModel.WhichCoursePrerequisites = course.WhichCoursePrerequisites;
            viewModel.Place = course.Place;
            viewModel.CoachName = course.CoachName;
            viewModel.CategoryId = course.CategoryId;
            viewModel.Status = course.Status;

            return viewModel;
        }

        public async Task<ServiceException> EditCourseFromAdmin(EditCourseViewModel editCourse)
        {
            var newCourse = await GetAsync(c => c.Id == editCourse.CourseId);

            if(newCourse == null)
                return ServiceException.Create(
                    type: "NotFound",
                    title: "موجودیت یافت نشد",
                    detail: "دوره ای با این شناسه یافت نشد.");

            newCourse.ImageName = editCourse.CourseImage;
            newCourse.CourseTitle = editCourse.CourseTitle;
            newCourse.Description = editCourse.Description;
            newCourse.Status = editCourse.Status;
            newCourse.CourseType = editCourse.CourseType;
            newCourse.DateOfHolding = DateTime.Parse(editCourse.DateOfHolding);
            newCourse.WhichCoursePrerequisites = editCourse.WhichCoursePrerequisites;
            newCourse.Place = editCourse.Place;
            newCourse.PrerequisiteCourse = editCourse.PrerequisiteCourse;
            newCourse.Pirce = decimal.Parse(editCourse.Pirce);
            newCourse.CategoryId = editCourse.CategoryId;
            newCourse.CoachName = editCourse.CoachName;

            #region Save Avatar

            if (editCourse.ImageName != null)
            {
                string imagePath = "";
                newCourse.ImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(editCourse.ImageName.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseImage", newCourse.ImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    editCourse.ImageName.CopyTo(stream);
                }
                newCourse.ImagePath = imagePath;
            }
            else
            {
                newCourse.ImageName = "Default.jpg";
            }

            #endregion

            await UpdateAsync(newCourse);
            await _unitOfWork.CommitAsync();

            return ServiceException.Create(
                    type: "Success",
                    title: "عملیات موفق",
                    detail: "عملیات ویرایش دوره با موفقیت انجام شد.");
        }

        public ServiceException DeleteCourse(int courseId)
        {
            var course = Get(c => c.Id == courseId);
            if(course == null)
                return ServiceException.Create(
                    type: "NotFound",
                    title: "عملیات ناموفق",
                    detail: "دوره ای با این شناسه یافت نشد.");

            course.IsDelete = true;
            _unitOfWork.Commit();

            return ServiceException.Create(
                    type: "Success",
                    title: "عملیات موفق",
                    detail: "عملیات حذف دوره با موفقیت انجام شد.");
        }

        public async Task<List<HeldCourseListViewModel>> GetHeldCourseList(int take)
        {
            IQueryable<Course> courses = await GetAllAsyncQuery();

             var courseList = courses.OrderByDescending(c => c.DateOfHolding)
                .Take(take).Select(c => new HeldCourseListViewModel
                {
                    Id = c.Id,
                    Title = c.CourseTitle,
                    DateOfHolding = c.DateOfHolding,
                    Image = c.ImageName,
                    Place = c.Place,
                    Status = c.Status,
                }).ToList();

            return courseList;
        }

        public async Task<CourseDetailViewModel> GetCourseDetails(int id)
        {
            var course = await GetAsync(c => c.Id==id);
            CourseDetailViewModel courseDetail = new CourseDetailViewModel()
            {
                Id = course.Id,
                Title = course.CourseTitle,
                Description = course.Description,
                Place = course.Place,
                CoathName = course.CoachName,
                Price = course.Pirce,
                HoldingDate = course.DateOfHolding.ToString("dd-MM-yyyy"),
                Image = course.ImageName,
                WhichCoursePrerequisites = course.WhichCoursePrerequisites,
                PrerequisiteCourse = course.PrerequisiteCourse,
            };
            switch (course.CourseType)
            {
                case DomainLayer.Enums.CourseType.Mountaineering:
                    courseDetail.CourseType = "کوهپیمایی";
                    break;
                case DomainLayer.Enums.CourseType.RockClimbing:
                    courseDetail.CourseType = "صخره نوردی";
                    break;
                case DomainLayer.Enums.CourseType.IceClimbing:
                    courseDetail.CourseType = "یخ نوردی";
                    break;
                case DomainLayer.Enums.CourseType.Rescue:
                    courseDetail.CourseType = "امداد و نجات";
                    break;
                default:
                    courseDetail.CourseType = "کوهپیمایی";
                    break;
            }

            return courseDetail;
        }
    }
}
