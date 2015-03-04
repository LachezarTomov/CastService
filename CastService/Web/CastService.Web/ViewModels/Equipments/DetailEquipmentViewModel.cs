namespace CastService.Web.ViewModels.Equipments
{
    
    using System.ComponentModel.DataAnnotations;
    
    using CastService.Data.Models;
    using CastService.Web.Infrastructure.Mapping;

    public class DetailEquipmentViewModel : IMapFrom<Equipment>
    {
        public int Id { get; set; }

        [Display(Name = "Име")]
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Името трябва да е между {1} и {2} символа.")]
        public string Name { get; set; }

        [Display(Name = "Модел")]
        [StringLength(16, MinimumLength = 3, ErrorMessage = "Моделът трябва да е между {1} и {2} символа.")]
        public string Model { get; set; }
    }
}