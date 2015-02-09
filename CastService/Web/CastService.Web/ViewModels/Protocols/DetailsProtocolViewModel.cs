namespace CastService.Web.ViewModels.Protocols
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using AutoMapper;

    using CastService.Data.Models;
    using CastService.Web.Infrastructure.Mapping;

    public class DetailsProtocolViewModel: IMapFrom<Installation>, IHaveCustomMappings
    {
        public int Id { get; set; }

        [Display(Name = "Машина тип")]
        [Required(ErrorMessage = "Полето е задължително")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Машина тип трябва да е между {1} и {2} символа")]
        public string ObjectType { get; set; }

        [Display(Name = "ДКН")]
        [Required(ErrorMessage = "Полето е задължително")]
        [StringLength(16, MinimumLength = 3, ErrorMessage = "ДКН трябва да е между {1} и {2} символа")]
        public string ObjectNumber { get; set; }

        [Display(Name = "Водач")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Водач трябва да е между {1} и {2} символа")]
        public string ObjectDriver { get; set; }

        [Display(Name = "Извършена диагностика")]
        [StringLength(1200, MinimumLength = 3, ErrorMessage = "Извършена диагностика трябва да е между {1} и {2} символа")]
        [DataType(DataType.MultilineText)]
        public string PerformedDiagnostic { get; set; }

        [Display(Name = "Открити неизправности")]
        [StringLength(1200, MinimumLength = 3, ErrorMessage = "Открити неизправности трябва да е между {1} и {2} символа")]
        [DataType(DataType.MultilineText)]
        public string DetectedFauls { get; set; }

        [Display(Name = "Час начало")]
        [Required(ErrorMessage = "Полето е задължително")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Час начало трябва да е точно 5 символа: чч:мм")]
        public string StartTime { get; set; }

        [Display(Name = "Час край")]
        [Required(ErrorMessage = "Полето е задължително")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Час край трябва да е точно 5 символа: чч:мм")]
        public string EndTime { get; set; }

        [Display(Name = "Дата")]
        [Required(ErrorMessage = "Полето е задължително")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Дата трябва да е точно 10 символа: дд/мм/гггг")]
        public string InstallationDate { get; set; }

        [Display(Name = "Обслужването е")]
        public bool IsWarrantyService { get; set; }

        [Display(Name = "")]
        public bool WithSubscriptionService { get; set; }

        [Display(Name = "Заявката е подадена от")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Заявката е подадена от трябва да е точно {1} символа")]
        public string PersonMadeRequest { get; set; }

        [Display(Name = "Дата на заявката")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Дата на заявката трябва да е точно {1} символа")]
        public string RequestDate { get; set; }

        [Display(Name = "Сервизен протокол")]
        public bool HasCustomerProtocol { get; set; }

        [Display(Name = "Фактура №")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Фактура № трябва да е между {1} и {2} символа")]
        public string InvoiceNumber { get; set; }

        [Display(Name = "Дата на фактурата")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Дата на фактурата трябва да е точно {1} символа")]
        public string InvoiceDate { get; set; }

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

        [Display(Name = "Вложен труд")]
        public int WorkInHours { get; set; }

        [Display(Name = "часа Х ")]
        public decimal PricePerHour { get; set; }

        [Display(Name = "Вложени части и материали")]
        public decimal PriceForChangedEguipment { get; set; }

        [Display(Name = "Изминати километри")]
        public decimal DistanceInKm { get; set; }

        [Display(Name = " Х ")]
        public decimal PricePerKm { get; set; }

        [Display(Name = "Клиент")]
        [UIHint("DropDownTwoRows")]
        public int CustomerId { get; set; }

        public IList<SelectListItem> CustomersNames { get; set; }

        public IList<ChangedEquipmentListViewModel> ChangedEquipment { get; set; }

        [Display(Name = "Клиент")]
        public string CustomerName { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Protocol, DetailsProtocolViewModel>()
             //   .ForMember(m => m.ProtocolDate, opt => opt.MapFrom(t => t.ProtocolDate.ToString()))
                .ForMember(m => m.InvoiceDate, opt => opt.MapFrom(t => t.InvoiceDate.ToString()))
                .ReverseMap();
        }
    }
}