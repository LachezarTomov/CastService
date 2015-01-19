namespace CastService.Web.ViewModels.Customers
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using CastService.Data.Models;
    using CastService.Web.Infrastructure.Mapping;
    

    public class CreateCustomerViewModel : IMapFrom<Customer>
    {
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
        public int OldNameId { get; set; }

        public IList<SelectListItem> CustomersNames { get; set; }
    }
}