using DomainLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTOs.Course
{
    public class CourseDetailViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Place { get; set; }
        public string? Description { get; set; }
        public string? CoathName { get; set; }
        public decimal? Price { get; set; }
        public string Image { get; set; }
        public string? WhichCoursePrerequisites { get; set; }
        public string? PrerequisiteCourse { get; set; }
        public string? HoldingDate { get; set; }
        public string CourseType { get; set; }
    }
}
