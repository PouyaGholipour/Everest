using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTOs.Report
{
    public class EditReportViewModel
    {
        public int ReportId { get; set; }

        [Display(Name = "عنوان گزارش")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string ReportTitle { get; set; }

        [Display(Name = "محتوا")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string ReportContent { get; set; }

        public IFormFile? ImageName { get; set; }
        public string? ReportImage { get; set; }
        public string? ImagePath { get; set; }
        public string Place { get; set; }
        public bool IsDelete { get; set; } = false;
        public string DateOfHolding { get; set; }
    }
}
