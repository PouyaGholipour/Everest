using DomainLayer.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTOs.Course
{
    public class EditCourseViewModel
    {
        public int CourseId { get; set; }

        [Display(Name = "عنوان دوره")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string CourseTitle { get; set; }

        [Display(Name = "توضیحات دوره")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = "نام مربی")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string CoachName { get; set; }

        [Display(Name = "مکان برگزاری")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Place { get; set; }

        [Display(Name = "پیش نیاز کدام دوره؟")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string? WhichCoursePrerequisites { get; set; }

        [Display(Name = "پیش نیاز دارد")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string? PrerequisiteCourse { get; set; }

        public IFormFile ImageName { get; set; }

        public string? CourseImage { get; set; }
        public string? ImagePath { get; set; }

        public int CategoryId { get; set; }

        [Display(Name = "هزینه دوره")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Pirce { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید")]
        public bool Status { get; set; }
        public string DateOfHolding { get; set; }
        public CourseType CourseType { get; set; }
    }
}
