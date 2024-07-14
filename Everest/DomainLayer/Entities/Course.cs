using DomainLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Course
    {
        public Course()
        {

        }
        public int Id { get; set; }
        public string CourseTitle { get; set; }
        public string Description { get; set; }
        public string CoachName { get; set; }
        public string Place { get; set; }
        public string? WhichCoursePrerequisites { get; set; }
        public string? PrerequisiteCourse { get; set; }
        public string? ImageName { get; set; }
        public string? ImagePath { get; set; }
        public decimal Pirce { get; set; }
        public bool Status { get; set; }
        public bool IsDelete { get; set; } = false;
        public DateTime DateOfHolding { get; set; }
        public CourseType CourseType { get; set; }

        #region Relations for navigation property

        public Category? Category { get; set; }
        public int CategoryId { get; set; }
        public ICollection<CourseUser>? CourseUsers { get; set; }
        public ICollection<Comment>? Comments { get; set; }

        #endregion
    }
}
