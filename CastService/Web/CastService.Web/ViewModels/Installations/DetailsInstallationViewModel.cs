﻿namespace CastService.Web.ViewModels.Installations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    
    using CastService.Data.Models;
    using CastService.Web.Infrastructure.Mapping;
   
    using AutoMapper;
    using System.Web.Mvc;
        
    public class DetailsInstallationViewModel : IMapFrom<Installation>, IHaveCustomMappings
    {
        public int Id { get; set; }

        [Display(Name = "Място")]
        [Required(ErrorMessage = "Полето е задължително")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Място трябва да е между {2} и {1} символа")]
        public string ObjectName { get; set; }

        [Display(Name = "Машина тип")]
        [Required(ErrorMessage = "Полето е задължително")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Машина тип трябва да е между {2} и {1} символа")]
        public string ObjectType { get; set; }

        [Display(Name = "ДКН")]
        [Required(ErrorMessage = "Полето е задължително")]
        [StringLength(16, MinimumLength = 3, ErrorMessage = "ДКН трябва да е между {2} и {1} символа")]
        public string ObjectNumber { get; set; }

        [Display(Name = "Основни дейности")]
        [StringLength(1200, MinimumLength = 3, ErrorMessage = "Основни дейности трябва да е между {2} и {1} символа")]
        [DataType(DataType.MultilineText)]
        public string DetectedFaults { get; set; }

        [Display(Name = "Допълнителни дейности")]
        [StringLength(1200, MinimumLength = 3, ErrorMessage = "Допълнителни дейности трябва да е между {2} и {1} символа")]
        [DataType(DataType.MultilineText)]
        public string AdditionalActivities { get; set; }

        [Display(Name = "Час начало")]
        [Required(ErrorMessage = "Полето е задължително")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Час начало трябва да е точно 5 символа: чч:мм")]
        public string StartTime { get; set; }

        [Display(Name = "Час край")]
        [Required(ErrorMessage = "Полето е задължително")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Час край трябва да е точно 5 символа: чч:мм")]
        public string EndTime { get; set; }

        [Display(Name = "Предвидено време")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Предвидено време трябва да е между {2} и {1} символа")]
        public string GuessedTime { get; set; }

        [Display(Name = "Дата")]
        [Required(ErrorMessage = "Полето е задължително")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Дата трябва да е точно 10 символа: дд/мм/гггг")]
        public string InstallationDate { get; set; }

        [Display(Name = "Приемо-предавателен протокол")]
        public bool HasProtocol { get; set; }

        [Display(Name = "Фактура №")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Фактура № трябва да е между {2} и {1} символа")]
        public string InvoiceNumber { get; set; }

        [Display(Name = "Дата на фактурата")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Дата на фактурата трябва да е точно {1} символа")]
        public string InvoiceDate { get; set; }

        [Display(Name = "Гаранционна карта №")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Гаранционна карта № трябва да е между {2} и {1} символа")]
        public string WarrantyCardNumber { get; set; }

        [Display(Name = "Друго")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Друго трябва да е между {2} и {1} символа")]
        public string Other { get; set; }

        [Display(Name = "Вложен труд")]
        [DataType(DataType.Text)]
        public int WorkInHours { get; set; }

        public decimal PricePerHour { get; set; }

        [Display(Name = "Вложени части и материали")]
        public decimal PriceForChangedEguipment { get; set; }

        [Display(Name = "Забележка")]
        [StringLength(1200, MinimumLength = 3, ErrorMessage = "Забележка трябва да е между {2} и {1} символа")]
        [DataType(DataType.MultilineText)]
        public string Note { get; set; }

        [Display(Name = "Клиент")]
        [UIHint("DropDownTwoRows")]
        public int CustomerId { get; set; }

        public IList<SelectListItem> CustomersNames { get; set; }

        public IList<InstalledEquipmentListViewModel> InstalledEquipment { get; set; }

        [Display(Name = "Клиент")]
        public string CustomerName { get; set; }

        [Display(Name = "Представител на изпълнителя")]
        [MaxLength(128)]
        [UIHint("UsersDropDownList")]
        public string UserId { get; set; }

        public IList<SelectListItem> UserNames { get; set; }

        [Display(Name = "Представител на изпълнителя")]
        public string UserName { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Installation, DetailsInstallationViewModel>()
                //        .ForMember(m => m.InstallationDate, opt => opt.MapFrom(t => t.InstallationDate.ToString("dd/MM/yyyy")))
                .ForMember(m => m.InvoiceDate, opt => opt.MapFrom(t => t.InvoiceDate.ToString()))

                .ReverseMap();
        }
    }
}