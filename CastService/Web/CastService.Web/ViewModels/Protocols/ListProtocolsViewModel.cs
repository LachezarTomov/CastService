namespace CastService.Web.ViewModels.Protocols
{
    using System;
    using System.Linq;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using CastService.Data.Models;
    using CastService.Web.Infrastructure.Mapping;
    using System.Collections.Generic;

    public class ListProtocolsViewModel : IMapFrom<Protocol>, IHaveCustomMappings
    {
        public int Id { get; set; }

        [Display(Name = "Име на клиента")]
        public string CustomerName { get; set; }

        [Display(Name = "Машина тип")]
        public string ObjectType { get; set; }

        [Display(Name = "ДКН")]
        public string ObjectNumber { get; set; }

        [Display(Name = "Дата")]
        public DateTime ProtocolDate { get; set; }

        public ICollection<ChangedEquipment> ChangedEquipment { get; set; }

        [Display(Name = "Оборудване")]
        public string Equipment { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Protocol, ListProtocolsViewModel>()
                .ForMember(m => m.CustomerName, opt => opt.MapFrom(t => t.Customer.Name))
                .ForMember(m => m.ChangedEquipment, opt => opt.MapFrom(src => src.ChangedEquipment.Where(x => x.IsDeleted == false)))
                .ReverseMap();
        }
    }
}