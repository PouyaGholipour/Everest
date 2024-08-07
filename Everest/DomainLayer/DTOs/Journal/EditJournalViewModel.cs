using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTOs.Journal
{
    public class EditJournalViewModel
    {
        public int JournalId { get; set; }

        [Display(Name = "عنوان برنامه")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string JournalTitle { get; set; }

        [Display(Name = "متن مجله")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید")]
        public string Content { get; set; }
        public IFormFile? ImageName { get; set; }
        public string? ImagePath { get; set; }
        public string? JournalImage { get; set; }
    }
}
