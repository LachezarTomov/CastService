namespace CastService.Web.ViewModels.Installations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    
    using CastService.Data.Models;
    using CastService.Web.Infrastructure.Mapping;
   
    using AutoMapper;
    using System.Web.Mvc;
        
    public class DetailsInstallationViewModel : IMapFrom<Installation>
    {
        public int Id { get; set; }

        [Display(Name = "Място")]
        [Required(ErrorMessage = "Полето е задължително")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Място трябва да е между {1} и {2} символа")]
        public string ObjectName { get; set; }

        [Display(Name = "Открити неизправности")]
        [StringLength(1200, MinimumLength = 3, ErrorMessage = "Открити неизправности трябва да е между {1} и {2} символа")]
        [DataType(DataType.MultilineText)]
        public string DetectedFaults { get; set; }

        [Display(Name = "Допълнителни дейности")]
        [StringLength(1200, MinimumLength = 3, ErrorMessage = "Допълнителни дейности трябва да е между {1} и {2} символа")]
        [DataType(DataType.MultilineText)]
        public string AdditionalActivities { get; set; }

        [Display(Name = "Час начало")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Час начало трябва да е точно 5 символа: чч:мм")]
        public string StartTime { get; set; }

        [Display(Name = "Час край")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Час край трябва да е точно 5 символа: чч:мм")]
        public string EndTime { get; set; }

        [Display(Name = "Предвидено време")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Предвидено време трябва да е между {1} и {2} символа")]
        public string GuessedTime { get; set; }

        [Display(Name = "Дата")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Дата трябва да е точно 10 символа: дд.мм.гггг")]
        public DateTime InstallationDate { get; set; }

        [Display(Name = "Приемо-предавателен протокол")]
        public bool HasProtocol { get; set; }

        [Display(Name = "Фактура №")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Фактура № трябва да е между {1} и {2} символа")]
        public string InvoiceNumber { get; set; }

        [Display(Name = "Дата на фактурата")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Дата на фактурата трябва да е точно {1} символа")]
        public DateTime? InvoiceDate { get; set; }

        [Display(Name = "Гаранционна карта №")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Гаранционна карта № трябва да е между {1} и {2} символа")]
        public string WarrantyCardNumber { get; set; }

        [Display(Name = "Друго")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Друго трябва да е между {1} и {2} символа")]
        public string Other { get; set; }

        [Display(Name = "Забележка")]
        [StringLength(1200, MinimumLength = 3, ErrorMessage = "Забележка трябва да е между {1} и {2} символа")]
        [DataType(DataType.MultilineText)]
        public string Note { get; set; }

        [Display(Name = "Клиент")]
        [UIHint("DropDownTwoRows")]
        public int CustomerId { get; set; }
        public IList<SelectListItem> CustomersNames { get; set; }

     //   public string CustomerName { get; set; }

        public IEnumerable<InstallatedEquipment> InstalledEquipment { get; set; }

        //public void CreateMappings(IConfiguration configuration)
        //{
        //    configuration.CreateMap<Installation, ListInstallationsViewModel>()
        //        .ForMember(m => m.CustomerName, opt => opt.MapFrom(t => t.Customer.Name))
        //        .ReverseMap();
        //}
    }
}