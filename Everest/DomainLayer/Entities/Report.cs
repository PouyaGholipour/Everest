using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public string ReportTitle { get; set; }
        public string ReportContent { get; set; }
        public string? ImageName { get; set; }
        public string? ImagePath { get; set; }
        public string Place { get; set; }
        public bool IsDelete { get; set; } = false;
        public DateTime DateOfHolding { get; set; }

        #region Relations for navigation property

        public ICollection<Comment> Comments { get; set; }

        #endregion

    }
}
