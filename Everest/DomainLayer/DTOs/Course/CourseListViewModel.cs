using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTOs.Course
{
    public class CourseListViewModel
    {
        public List<Entities.Course> Courses { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public string? CourseTitleFilter { get; set; }
        public int PageId { get; set; }
    }
}
