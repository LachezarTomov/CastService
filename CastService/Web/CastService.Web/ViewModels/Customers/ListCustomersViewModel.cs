namespace CastService.Web.ViewModels.Customers
{
    using CastService.Data.Models;
    using CastService.Web.Infrastructure.Mapping;

    public class ListCustomersViewModel : IMapFrom<Customer>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Place { get; set; }

        public string Address { get; set; }
        
        public string Note { get; set; }
    }
}