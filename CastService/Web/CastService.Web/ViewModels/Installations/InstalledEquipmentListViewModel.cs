namespace CastService.Web.ViewModels.Installations
{
    using System.Web.Mvc;

    using CastService.Data.Models;
    using CastService.Web.Infrastructure.Mapping;

    public class InstalledEquipmentListViewModel : IMapFrom<InstalledEquipment>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string EquipmentName { get; set; }

        public string SerialNumber { get; set; }
        
        public int Quantity { get; set; }

        public int EquipmentLength { get; set; }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<InstalledEquipment, InstalledEquipmentListViewModel>()
                .ForMember(m => m.Id, opt => opt.MapFrom(t => t.Equipment.Id))
                .ForMember(m => m.EquipmentName, opt => opt.MapFrom(t => t.Equipment.Name))
                .ReverseMap();
        }
    }
}