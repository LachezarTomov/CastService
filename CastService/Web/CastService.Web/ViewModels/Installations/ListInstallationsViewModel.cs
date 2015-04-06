namespace CastService.Web.ViewModels.Installations
{
    using System;
    using System.Linq;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using CastService.Data.Models;
    using CastService.Web.Infrastructure.Mapping;
    using System.Collections.Generic;
    
    public class ListInstallationsViewModel : IMapFrom<Installation>, IHaveCustomMappings
    {
        public int Id { get; set; }

        [Display(Name = "Име на клиента")]
        public string CustomerName { get; set; }

        [Display(Name = "Място")]
        public string ObjectName { get; set; }

        [Display(Name = "Машина №")]
        public string ObjectNumber { get; set; }

        [Display(Name = "Дата")]
        public DateTime InstallationDate { get; set; }

        public ICollection<InstalledEquipment> InstallatedEquipment { get; set; }

        [Display(Name = "Оборудване")]
        public string Equipment { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Installation, ListInstallationsViewModel>()
                .ForMember(m => m.CustomerName, opt => opt.MapFrom(t => t.Customer.Name))
                .ForMember(m => m.InstallatedEquipment, opt => opt.MapFrom(src => src.InstalatedEquipment.Where(x => x.IsDeleted == false)))
            //    .ForMember(m => m.InstallationDate, opt => opt.MapFrom(t => t.InstallationDate.ToString()))
                .ReverseMap();
        }
    }
}