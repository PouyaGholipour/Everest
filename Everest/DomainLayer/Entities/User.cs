using DomainLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string? NationalCode { get; set; }
        public string? ImageName { get; set; }
        public string? ImagePath { get; set; }
        public bool IsActive { get; set; } = false;
        public bool IsDelete { get; set; } = false;
        public DateTime RegisterDate { get; set; }
        public DateTime BirthDayDate { get; set; }
        public UserType UserType { get; set; }

        #region Relations for navigation property

        public virtual Prog Prog { get; set; }
        public int ProgId { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        #endregion
    }
}
