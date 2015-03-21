namespace CastService.Web.ViewModels.WaitingsService
{
    using System;
    using System.Linq;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using CastService.Data.Models;
    using CastService.Web.Infrastructure.Mapping;
    using System.Collections.Generic;

    public class ListWaitingsServiceViewModel : IMapFrom<WaitingService>, IHaveCustomMappings
    {
        public int Id { get; set; }

        [Display(Name = "ДКН")]
        public string ObjectNumber { get; set; }

        [Display(Name = "Име на клиента")]
        public string CustomerName { get; set; }

        [Display(Name = "Подаден на дата")]
        public DateTime RequestDate { get; set; }

        [Display(Name = "Подал информацията")]
        public string SubmittedInfo { get; set; }

        [Display(Name = "Описание на проблема")]
        public string ProblemDescription { get; set; }

        [Display(Name = "Планиран за")]
        public DateTime PlannedDate { get; set; }

        [Display(Name = "Планиран специалист")]
        public string PlannedSpecialist { get; set; }

        [Display(Name = "Извършен")]
        public bool IsDone { get; set; }

        public string UserId { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<WaitingService, ListWaitingsServiceViewModel>()
                .ReverseMap();
        }
    }
}