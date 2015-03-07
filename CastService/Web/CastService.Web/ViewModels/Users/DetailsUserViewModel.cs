namespace CastService.Web.ViewModels.Users
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using CastService.Data.Models;
    using CastService.Web.Infrastructure.Mapping;

    using AutoMapper;
    using System.Web.Mvc;

    public class DetailsUserViewModel : IMapFrom<User>
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [StringLength(10, ErrorMessage = "{0} трябва да бъде най-малко {2} символа..", MinimumLength = 3)]
        [Display(Name = "Име")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [StringLength(100, ErrorMessage = "{0} трябва да бъде най-малко {2} символа.", MinimumLength = 6)]
        [Display(Name = "Пълно име")]
        public string FullName { get; set; }

        [Display(Name = "Група")]
        public string RoleId { get; set; }
        public System.Web.Mvc.SelectList Roles { get; set; }

        [Display(Name = "Блокиран")]
        public bool IsBlocked { get; set; }
    }
}