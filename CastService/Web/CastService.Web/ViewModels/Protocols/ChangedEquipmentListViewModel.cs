namespace CastService.Web.ViewModels.Protocols
{
    using System;

    using CastService.Data.Models;
    using CastService.Web.Infrastructure.Mapping;
    

    public class ChangedEquipmentListViewModel : IMapFrom<ChangedEquipment>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string EquipmentName { get; set; }

        public string OldSerialNumber { get; set; }

        public string NewSerialNumber { get; set; }

        public int Quantity { get; set; }

        public int EquipmentLength { get; set; }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<ChangedEquipment, ChangedEquipmentListViewModel>()
                .ForMember(m => m.Id, opt => opt.MapFrom(t => t.Equipment.Id))
                .ForMember(m => m.EquipmentName, opt => opt.MapFrom(t => t.Equipment.Name))
                .ReverseMap();
        }
    }
}