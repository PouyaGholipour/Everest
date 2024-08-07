using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTOs.CourseUser
{
    public class AddCourseUser
    {
        public int CourseId { get; set; }
        public int UserId { get; set; }
    }
}
