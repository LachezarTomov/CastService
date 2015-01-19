namespace CastService.Web.ViewModels.Customers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    using AutoMapper.QueryableExtensions;

    using CastService.Web.Infrastructure.Mapping;
    using CastService.Data.Models;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class DetailsCustomerViewModel : IMapFrom<Customer>
    {
        public int Id { get; set; }

        [Display(Name = "Име")]
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Името трябва да е между {1} и {2} символа.")]
        public string Name { get; set; }

        [Display(Name = "ЕИК")]
        [StringLength(16, MinimumLength = 9, ErrorMessage = "ЕИК трябва да е между {1} и {2} символа.")]
        public string Eik { get; set; }

        [Display(Name = "Място")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Мястото трябва да е между {1} и {2} символа.")]
        public string Place { get; set; }

        [Display(Name = "Адрес")]
        [StringLength(1200, MinimumLength = 3, ErrorMessage = "Адресът трябва да е между {1} и {2} символа.")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "Забележка")]
        [StringLength(1200, MinimumLength = 3, ErrorMessage = "Забележката трябва да е между {1} и {2} символа.")]
        [DataType(DataType.MultilineText)]
        public string Note { get; set; }

        [Display(Name = "Предишно име")]
        [UIHint("DropDownList")]
        public int? OldNameId { get; set; }

        public IList<SelectListItem> CustomersNames { get; set; }

        public string OldName { get; set; }
    }
}