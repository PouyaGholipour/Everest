using DomainLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CommentDescription { get; set; }
        public int? Like { get; set; }
        public int? DisLike { get; set; }
        public bool IsDelete { get; set; } = false;
        public DateTime InsertDate { get; set; }
        public CommentStatus CommentStatus { get; set; }

        #region Relations for navigation property

        public User User { get; set; }
        public int UserId { get; set; }
        public Report Report { get; set; }
        public int ReportId { get; set; }
        public Course Course { get; set; }
        public int CourseId { get; set; }

        #endregion
    }
}
