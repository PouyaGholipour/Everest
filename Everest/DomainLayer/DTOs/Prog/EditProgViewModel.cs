using DomainLayer.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTOs.Prog
{
    public class EditProgViewModel
    {
        public int ProgId { get; set; }

        [Display(Name = "عنوان برنامه")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "توضیحات برنامه")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = "نام سرپرست")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string? LeaderName { get; set; }

        [Display(Name = "مکان برگزاری")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Place { get; set; }

        [Display(Name = "پیش نیاز کدام برنامه؟")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string? WhichProgramPrerequisites { get; set; }

        [Display(Name = "پیش نیاز دارد- برنامه")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string? PrerequisitePrograms { get; set; }

        [Display(Name = "پیش نیاز دارد- دوره")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string? PrerequisiteCourse { get; set; }

        public IFormFile? ImageName { get; set; }
        public string? ImagePath { get; set; }
        public string? ProgImage { get; set; }

        [Display(Name = "هزینه دوره")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Price { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید")]
        public bool Status { get; set; }
        public string DateOfHolding { get; set; }
        public ProgramType ProgramType { get; set; }
    }
}
