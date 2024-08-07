using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTOs.Course
{
    public class GetCourseForUserViewModel
    {
        public int? CourseId { get; set; }
        public string? CourseTitle { get; set; }
        public string? Description { get; set; }
        public string? HoldingDate { get; set; }
    }
}
