namespace CastService.Web.ViewModels.Customers
{
    using CastService.Data.Models;
    using CastService.Web.Infrastructure.Mapping;
    using System.ComponentModel.DataAnnotations;

    public class ListCustomersViewModel : IMapFrom<Customer>
    {
        public int Id { get; set; }

        [Display(Name = "Име")]
        public string Name { get; set; }

        [Display(Name = "Място")]
        public string Place { get; set; }

        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Display(Name = "Забележка")]
        public string Note { get; set; }
    }
}