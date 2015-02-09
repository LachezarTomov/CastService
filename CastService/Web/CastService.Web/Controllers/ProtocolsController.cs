namespace CastService.Web.Controllers
{
        using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Net;
    using System.Text;

    using AutoMapper.QueryableExtensions;
    using PagedList;

    using CastService.Data.Common.Repository;
    using CastService.Data.Models;
    using CastService.Web.ViewModels.Protocols;
    using CastService.Web.Infrastructure.Populators;

    public class ProtocolsController : Controller
    {

        private readonly IDeletableEntityRepository<Protocol> protocols;
        private readonly DropDownListPopulator populator;

        public ProtocolsController(
            IDeletableEntityRepository<Protocol> protocols, DropDownListPopulator populator)
        {
            this.protocols = protocols;
            this.populator = populator;
        }
        // GET: Protocols
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            var model = this.protocols.All().Project().To<ListProtocolsViewModel>();

            ViewBag.ClientNameSortParams = sortOrder == "clientName" ? "clientNameDesc" : "clientName";
            ViewBag.MachineTypeSortParams = sortOrder == "machineType" ? "machineTypeDesc" : "machineType";
            ViewBag.MachineNumberSortParams = sortOrder == "machineNumber" ? "machineNumberDesc" : "machineNumber";
            ViewBag.DateSortParams = string.IsNullOrEmpty(sortOrder) ? "date" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(i => i.CustomerName.ToLower().Contains(searchString.ToLower()));
            }

            switch (sortOrder)
            {
                case "clientName":
                    model = model.OrderBy(o => o.CustomerName);
                    break;
                case "clientNameDesc":
                    model = model.OrderByDescending(o => o.CustomerName);
                    break;
                case "machineType":
                    model = model.OrderBy(o => o.ObjectType);
                    break;
                case "machineTypeDesc":
                    model = model.OrderByDescending(o => o.ObjectType);
                    break;
                case "machineNumber":
                    model = model.OrderBy(o => o.ObjectNumber);
                    break;
                case "machineNumberDesc":
                    model = model.OrderByDescending(o => o.ObjectNumber);
                    break;
                case "date":
                    model = model.OrderBy(o => o.ProtocolDate);
                    break;
                default:
                    model = model.OrderByDescending(o => o.ProtocolDate);
                    break;
            }

            int pageSize = 2;
            int pageNumber = (page ?? 1);

            return View(model.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {            
            var protocolViewModel = new DetailsProtocolViewModel();
            protocolViewModel.CustomersNames = this.populator.PopulateCustomers();

            return View(protocolViewModel);
        }
    }
}