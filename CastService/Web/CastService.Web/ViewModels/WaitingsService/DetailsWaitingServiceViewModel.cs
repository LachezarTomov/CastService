namespace CastService.Web.ViewModels.WaitingsService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using CastService.Data.Models;
    using CastService.Web.Infrastructure.Mapping;

    using AutoMapper;
    using System.Web.Mvc;

    public class DetailsWaitingServiceViewModel : IMapFrom<WaitingService>
    {
        public int Id { get; set; }

        [Display(Name = "ДКН")]
        [Required(ErrorMessage = "Полето е задължително")]
        [StringLength(16, MinimumLength = 3, ErrorMessage = "ДКН трябва да е между {2} и {1} символа")]
        public string ObjectNumber { get; set; }

        [Display(Name = "Клиент")]
        [UIHint("DropDownList")]
        public int CustomerId { get; set; }

        public IList<SelectListItem> CustomersNames { get; set; }

        [Display(Name = "Заявен на")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Дата на заявката трябва да е точно {1} символа")]
        public string RequestDate { get; set; }

        [Display(Name = "Подал информацията")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Подал информацията трябва да е между {2} и {1} символа")]
        public string SubmittedInfo { get; set; }

        [Display(Name = "Описание на проблема")]
        [StringLength(1200, MinimumLength = 3, ErrorMessage = "Описание на проблема трябва да е между {2} и {1} символа")]
        [DataType(DataType.MultilineText)]
        public string ProblemDescription { get; set; }

        [Display(Name = "Планиран за")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Планиран за ремонт трябва да е точно {1} символа")]
        public string PlannedDate { get; set; }

        [Display(Name = "Планиран специалист")]
        [UIHint("UsersListOneRow")]
        public string UserId { get; set; }

        public IList<SelectListItem> PlannedSpecialist { get; set; }

        [Display(Name = "Извършен")]
        public bool IsDone { get; set; }
    }
}