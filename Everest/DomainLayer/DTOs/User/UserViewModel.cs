using DomainLayer.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTOs.User
{
    public class UserListViewModel
    {
        public List<Entities.User> Users { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public string UserNameFilter { get; set; }
        public string EmailFilter { get; set; }
        public int PageId { get; set; }
    }

    public class EditUserViewModel
    {
        public int UserId { get; set; }

        [Display(Name = "ایمیل")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Email { get; set; }

        public string? UserName { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید")]
        [MaxLength(20, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string? Password { get; set; }

        public IFormFile? ImageName { get; set; }

        public string? AvatarName { get; set; }

        public List<int>? UserRoles { get; set; }

    }

    public class CreateUserViewModel
    {

        [Display(Name = "ایمیل")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Email { get; set; }

        [Display(Name = "نام کاربری")]
        [MaxLength(20, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string UserName { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید")]
        [MaxLength(20, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Password { get; set; }

        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید")]
        [MaxLength(20, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [Compare(nameof(Password), ErrorMessage = "کلمه های عبور مغایرت دارند.")]
        public string RePassword { get; set; }

        public IFormFile ImageName { get; set; }
    }

    public class EditUserInformationViewModel
    {
        public string UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? BirthdayDate { get; set; }
        public IFormFile? ImageName { get; set; }
        public string? UserImage { get; set; }
        public string? NationalCode { get; set; }
    }
}
