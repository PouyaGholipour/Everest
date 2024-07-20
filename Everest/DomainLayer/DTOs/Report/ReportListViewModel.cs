using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTOs.Report
{
    public class ReportListViewModel
    {
        public List<Entities.Report> Reports { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public string? ReportTitleFilter { get; set; }
        public int PageId { get; set; }
    }
}
