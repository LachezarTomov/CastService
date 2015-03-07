namespace CastService.Web.ViewModels.Users
{
    using System;
    using System.Linq;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using System.ComponentModel.DataAnnotations;

    using CastService.Data.Models;
    using CastService.Web.Infrastructure.Mapping;

    public class ListUsersViewModel : IMapFrom<User>, IHaveCustomMappings
    {
        public string Id { get; set; }

        [Display(Name = "Име")]
        public string UserName { get; set; }

        [Display(Name = "Пълно име")]
        public string FullName { get; set; }

        [Display(Name = "Група")]
        public string Role { get; set; }

        public string RoleId { get; set; }

        [Display(Name = "Блокиран")]
        public bool IsBlocked { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<User, ListUsersViewModel>()
            //    .ForMember(m => m.Role, opt => opt.MapFrom(t => t.Roles))
                .ReverseMap();
        }
    }
}