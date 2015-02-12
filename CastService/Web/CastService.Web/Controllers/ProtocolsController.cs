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
    using System.Data.Entity.Validation;

    public class ProtocolsController : Controller
    {

        private readonly IDeletableEntityRepository<Protocol> protocols;
        private readonly IDeletableEntityRepository<ChangedEquipment> changedEquipment;
        private readonly DropDownListPopulator populator;

        public ProtocolsController(
            IDeletableEntityRepository<Protocol> protocols,
            IDeletableEntityRepository<ChangedEquipment> changedEquipment, 
            DropDownListPopulator populator)
        {
            this.protocols = protocols;
            this.changedEquipment = changedEquipment;
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
            protocolViewModel.UserNames = this.populator.PopulateUsers();

            return View(protocolViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DetailsProtocolViewModel protocol)
        {
            if (ModelState.IsValid)
            {
                var newProtocol = new Protocol();

                newProtocol.ObjectType = protocol.ObjectType;
                newProtocol.ObjectDriver = (protocol.ObjectDriver ?? string.Empty);
                newProtocol.ObjectNumber = protocol.ObjectNumber;
                newProtocol.ProtocolDate = DateTime.Parse(protocol.ProtocolDate);
                newProtocol.StartTime = protocol.StartTime;
                newProtocol.EndTime = protocol.EndTime;
                newProtocol.PerformedDiagnostic = protocol.PerformedDiagnostic ?? string.Empty;
                newProtocol.DetectedFauls = protocol.DetectedFauls ?? string.Empty;
                newProtocol.CustomerId = protocol.CustomerId;
                newProtocol.Note = protocol.Note ?? string.Empty;
                newProtocol.WorkInHours = protocol.WorkInHours;
                newProtocol.PricePerHour = protocol.PricePerHour;
                newProtocol.PriceForChangedEguipment = protocol.PriceForChangedEguipment;
                newProtocol.DistanceInKm = protocol.DistanceInKm;
                newProtocol.PricePerKm = protocol.PricePerKm;
                newProtocol.PersonMadeRequest = protocol.PersonMadeRequest ?? string.Empty;
                newProtocol.WarrantyCardNumber = protocol.WarrantyCardNumber ?? string.Empty;
                newProtocol.CustomerRepresentative = protocol.CustomerRepresentative ?? string.Empty;

                newProtocol.UserId = protocol.UserId;
               
                if (protocol.HasCustomerProtocol)
                {
                    newProtocol.HasCustomerProtocol = protocol.HasCustomerProtocol;
                }

                newProtocol.InvoiceNumber = (protocol.InvoiceNumber ?? string.Empty);
                
                if (protocol.InvoiceDate != null)
                {
                    newProtocol.InvoiceDate = DateTime.Parse(protocol.InvoiceDate);
                }
                else
                {
                    newProtocol.InvoiceDate = null;
                }
                if (protocol.RequestDate != null)
                {
                    newProtocol.RequestDate = DateTime.Parse(protocol.RequestDate);
                }
                else
                {
                    newProtocol.RequestDate = null;
                }

                newProtocol.Other = protocol.Other ?? string.Empty;
                this.protocols.Add(newProtocol);
                //this.protocols.SaveChanges();

                try
                {
                    this.protocols.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

                if (protocol.ChangedEquipment != null)
                {
                    foreach (var item in protocol.ChangedEquipment)
                    {
                        ChangedEquipment chEquipment = new ChangedEquipment();
                        chEquipment.ProtocolId = newProtocol.Id;
                        chEquipment.EquipmentId = item.Id;
                        chEquipment.OldSerialNumber = item.OldSerialNumber;
                        chEquipment.NewSerialNumber = item.NewSerialNumber;
                        chEquipment.Quantity = item.Quantity;

                        this.changedEquipment.Add(chEquipment);
                    }

                    this.changedEquipment.SaveChanges();
                }

                TempData["message"] = "Протоколът беше записан";

                return RedirectToAction("Index");
            }

            protocol.CustomersNames = this.populator.PopulateCustomers();
            protocol.UserNames = this.populator.PopulateUsers();

            return View(protocol);
        }
    }
}