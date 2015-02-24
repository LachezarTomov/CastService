using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
//using System.Web.Mvc;

namespace CastService.Web.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Име")]
        public string Username { get; set; }

        //[Required]
        //[Display(Name = "Email")]
        //[EmailAddress]
        //public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; }

        [Display(Name = "Запомни ме!")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "{0} трябва да бъде най-малко {2} символа..", MinimumLength = 3)]
        [Display(Name = "Име")]
        public string Username { get; set; }

        //[Required]
        //[EmailAddress]
        //[Display(Name = "Email")]
        //public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} трябва да бъде най-малко {2} символа.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} трябва да бъде най-малко {2} символа.", MinimumLength = 6)]
        [Display(Name = "Пълно име")]
        public string FullName { get; set; }

        [Display(Name = "Група")]
        public string RoleId { get; set; }
        public System.Web.Mvc.SelectList Roles { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Потвърди паролата")]
        [Compare("Password", ErrorMessage = "Двете пароли са различни!")]
        public string ConfirmPassword { get; set; }


    }

    public class EditViewModel
    {
        public string Id { get; set; }

        public bool HasPassword { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "{0} трябва да бъде най-малко {2} символа..", MinimumLength = 3)]
        [Display(Name = "Име")]
        public string Username { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "{0} трябва да бъде най-малко {2} символа.", MinimumLength = 6)]
        [Display(Name = "Пълно име")]
        public string FullName { get; set; }


        [Display(Name = "Група")]
        public string RoleId { get; set; }
        public System.Web.Mvc.SelectList Roles { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} трябва да бъде най-малко {2} символа..", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Двете пароли са различни!")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "Име")]
        public string Username { get; set; }
    }
}
