namespace CastService.Web.ViewModels.Equipments
{
    using System.ComponentModel.DataAnnotations;

    using CastService.Data.Models;
    using CastService.Web.Infrastructure.Mapping;

    public class ListEquipmentsViewModel : IMapFrom<Equipment>
    {
        public int Id { get; set; }

        [Display(Name = "Име")]
        public string Name { get; set; }

        [Display(Name = "Модел")]
        public string Model { get; set; }
    }
}