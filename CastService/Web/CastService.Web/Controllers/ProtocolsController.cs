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

    [Authorize]
    public class ProtocolsController : Controller
    {

        private readonly IDeletableEntityRepository<Protocol> protocols;
        private readonly IDeletableEntityRepository<ChangedEquipment> changedEquipment;
        private readonly IDeletableEntityRepository<Customer> customers;
        private readonly IDeletableEntityRepository<User> users;
        private readonly DropDownListPopulator populator;

        public ProtocolsController(
            IDeletableEntityRepository<Protocol> protocols,
            IDeletableEntityRepository<ChangedEquipment> changedEquipment,
            IDeletableEntityRepository<Customer> customers,
            IDeletableEntityRepository<User> users, 
            DropDownListPopulator populator)
        {
            this.protocols = protocols;
            this.changedEquipment = changedEquipment;
            this.customers = customers;
            this.users = users;
            this.populator = populator;
        }

        // GET: Protocols
        public ActionResult Index(string sortOrder, string currentFilter, int? page,
            string searchByCustomerName, string searchByObjectNumber, string searchByObjectType,
            string searchByChangedPartName, string searchByChangedPartSerNum)
        {
            var model = this.protocols.All().Project().To<ListProtocolsViewModel>();

            ViewBag.ClientNameSortParams = sortOrder == "clientName" ? "clientNameDesc" : "clientName";
            ViewBag.MachineTypeSortParams = sortOrder == "machineType" ? "machineTypeDesc" : "machineType";
            ViewBag.MachineNumberSortParams = sortOrder == "machineNumber" ? "machineNumberDesc" : "machineNumber";
            ViewBag.DateSortParams = string.IsNullOrEmpty(sortOrder) ? "date" : "";

            ViewBag.searchByCustomerName = searchByCustomerName;
            ViewBag.searchByObjectNumber = searchByObjectNumber;
            ViewBag.searchByObjectType = searchByObjectType;
            ViewBag.searchByChangedPartName = searchByChangedPartName;
            ViewBag.searchByChangedPartSerNum = searchByChangedPartSerNum;

            //if (searchByCustomerName != null)
            //{
            //    page = 1;
            //}
            //else
            //{
            //    searchByCustomerName = currentFilter;
            //}

            // searching strings
            if (!string.IsNullOrEmpty(searchByCustomerName))
            {
                model = model.Where(i => i.CustomerName.ToLower().Contains(searchByCustomerName.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchByObjectNumber))
            {
                model = model.Where(i => i.ObjectNumber.ToLower().Contains(searchByObjectNumber.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchByObjectType))
            {
                model = model.Where(i => i.ObjectType.ToLower().Contains(searchByObjectType.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchByChangedPartName))
            {
                model = model.Where(i => i.ChangedEquipment.Any(cheq => cheq.Equipment.Name.ToLower().Contains(searchByChangedPartName.ToLower())));
            }
            if (!string.IsNullOrEmpty(searchByChangedPartSerNum))
            {
                model = model.Where(i => i.ChangedEquipment.Any(
                    (c => (c.NewSerialNumber.ToLower().Contains(searchByChangedPartSerNum.ToLower())) ||
                           c.OldSerialNumber.ToLower().Contains(searchByChangedPartSerNum.ToLower()))
                    ));
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

            int pageSize = 15;
            int pageNumber = (page ?? 1);

            return View(model.ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "Администратор,Редактор")]
        public ActionResult Create()
        {            
            var protocolViewModel = new DetailsProtocolViewModel();
            protocolViewModel.CustomersNames = this.populator.PopulateCustomers();
            protocolViewModel.UserNames = this.populator.PopulateUsers();

            return View(protocolViewModel);
        }

        [Authorize(Roles = "Администратор,Редактор")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DetailsProtocolViewModel protocolModel)
        {
            if (ModelState.IsValid)
            {
                DateTime protocolDate = DateTime.Parse(protocolModel.ProtocolDate);
                var duplicateProtocolCheck = this.protocols.All()
                    .Where(c => c.CustomerId == protocolModel.CustomerId)
                    .Where(c => c.ProtocolDate == protocolDate)
                    .Where(c => c.StartTime == protocolModel.StartTime)
                    .FirstOrDefault();

                if (duplicateProtocolCheck != null)
                {
                    protocolModel.CustomersNames = this.populator.PopulateCustomers(protocolModel.CustomerId);
                    protocolModel.UserNames = this.populator.PopulateUsers(protocolModel.UserId);

                    TempData["message"] = "Протокол за този клиент със същата дата и час вече съществува";

                    return View(protocolModel);
                }

                var newProtocol = new Protocol();

                newProtocol.ObjectType = protocolModel.ObjectType;
                newProtocol.ObjectDriver = (protocolModel.ObjectDriver ?? string.Empty);
                newProtocol.ObjectNumber = protocolModel.ObjectNumber;
                newProtocol.ProtocolDate = DateTime.Parse(protocolModel.ProtocolDate);
                newProtocol.StartTime = protocolModel.StartTime;
                newProtocol.EndTime = protocolModel.EndTime;
                newProtocol.PerformedDiagnostic = protocolModel.PerformedDiagnostic ?? string.Empty;
                newProtocol.DetectedFauls = protocolModel.DetectedFauls ?? string.Empty;
                newProtocol.CustomerId = protocolModel.CustomerId;
                newProtocol.Note = protocolModel.Note ?? string.Empty;
                newProtocol.WorkInHours = protocolModel.WorkInHours;
                newProtocol.PricePerHour = protocolModel.PricePerHour;
                newProtocol.PriceForChangedEguipment = protocolModel.PriceForChangedEguipment;
                newProtocol.DistanceInKm = protocolModel.DistanceInKm;
                newProtocol.PricePerKm = protocolModel.PricePerKm;
                newProtocol.PersonMadeRequest = protocolModel.PersonMadeRequest ?? string.Empty;
                newProtocol.WarrantyCardNumber = protocolModel.WarrantyCardNumber ?? string.Empty;
                newProtocol.CustomerRepresentative = protocolModel.CustomerRepresentative ?? string.Empty;
                newProtocol.UserId = protocolModel.UserId;
                newProtocol.IsWarrantyService = protocolModel.IsWarrantyService;
                newProtocol.WithSubscriptionService = protocolModel.WithSubscriptionService;

                if (protocolModel.HasCustomerProtocol)
                {
                    newProtocol.HasCustomerProtocol = protocolModel.HasCustomerProtocol;
                }

                newProtocol.InvoiceNumber = (protocolModel.InvoiceNumber ?? string.Empty);

                if (protocolModel.InvoiceDate != null)
                {
                    newProtocol.InvoiceDate = DateTime.Parse(protocolModel.InvoiceDate);
                }
                else
                {
                    newProtocol.InvoiceDate = null;
                }

                if (protocolModel.RequestDate != null)
                {
                    newProtocol.RequestDate = DateTime.Parse(protocolModel.RequestDate);
                }
                else
                {
                    newProtocol.RequestDate = null;
                }

                newProtocol.Other = protocolModel.Other ?? string.Empty;
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

                if (protocolModel.ChangedEquipment != null)
                {
                    foreach (var item in protocolModel.ChangedEquipment)
                    {
                        ChangedEquipment chEquipment = new ChangedEquipment();
                        chEquipment.ProtocolId = newProtocol.Id;
                        chEquipment.EquipmentId = item.Id;
                        chEquipment.OldSerialNumber = item.OldSerialNumber;
                        chEquipment.NewSerialNumber = item.NewSerialNumber;
                        chEquipment.Quantity = item.Quantity;
                        chEquipment.EquipmentLength = item.EquipmentLength;

                        this.changedEquipment.Add(chEquipment);
                    }

                    this.changedEquipment.SaveChanges();
                }

                TempData["message"] = "Протоколът беше записан";

                return RedirectToAction("Index");
            }

            protocolModel.CustomersNames = this.populator.PopulateCustomers();
            protocolModel.UserNames = this.populator.PopulateUsers();

            return View(protocolModel);
        }

        [Authorize(Roles = "Администратор,Редактор")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var protocol2 = this.protocols.All().Where(c => c.Id == id).FirstOrDefault();

            var protocol = this.protocols.All().Where(c => c.Id == id).Project().To<DetailsProtocolViewModel>().FirstOrDefault();

            if (protocol == null)
            {
                return HttpNotFound();
            }

            protocol.CustomersNames = this.populator.PopulateCustomers(protocol.CustomerId);
            protocol.UserNames = this.populator.PopulateUsers(protocol.UserId);

            protocol.ProtocolDate = ConvertDbDate(protocol.ProtocolDate);
            if (!string.IsNullOrEmpty(protocol.InvoiceDate))
            {
                protocol.InvoiceDate = ConvertDbDate(protocol.InvoiceDate);
            }
            if (!string.IsNullOrEmpty(protocol.RequestDate))
            {
                protocol.RequestDate = ConvertDbDate(protocol.RequestDate);
            }

            protocol.ChangedEquipment = this.changedEquipment.All().Where(i => i.ProtocolId == protocol.Id).Project().To<ChangedEquipmentListViewModel>().ToList();

            return View(protocol);
        }

        [Authorize(Roles = "Администратор,Редактор")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DetailsProtocolViewModel protocolModel)
        {
            string expectedDateFormat = "дд/мм/гггг";
            if (ModelState.IsValid)
            {
                DateTime protocolDate = DateTime.Parse(protocolModel.ProtocolDate);
                var duplicateProtocolCheck = this.protocols.All()
                    .Where(c => c.CustomerId == protocolModel.CustomerId)
                    .Where(c => c.ProtocolDate == protocolDate)
                    .Where(c => c.StartTime == protocolModel.StartTime)
                    .FirstOrDefault();

                if (duplicateProtocolCheck != null)
                {
                    protocolModel.CustomersNames = this.populator.PopulateCustomers(protocolModel.CustomerId);
                    protocolModel.UserNames = this.populator.PopulateUsers(protocolModel.UserId);

                    TempData["message"] = "Протокол за този клиент със същата дата и час вече съществува";

                    return View(protocolModel);
                }

                var updatedProtocol = this.protocols.All().Where(c => c.Id == protocolModel.Id).FirstOrDefault();

                if (updatedProtocol == null)
                {
                    return HttpNotFound();
                }

                updatedProtocol.ObjectType = protocolModel.ObjectType;
                updatedProtocol.ObjectNumber = protocolModel.ObjectNumber;
                updatedProtocol.ObjectDriver = protocolModel.ObjectDriver;
                updatedProtocol.ProtocolDate = DateTime.Parse(protocolModel.ProtocolDate);
                updatedProtocol.StartTime = protocolModel.StartTime;
                updatedProtocol.EndTime = protocolModel.EndTime;
                updatedProtocol.PerformedDiagnostic = protocolModel.PerformedDiagnostic;
                updatedProtocol.DetectedFauls = protocolModel.DetectedFauls;
                updatedProtocol.IsWarrantyService = protocolModel.IsWarrantyService;
                updatedProtocol.WithSubscriptionService = protocolModel.WithSubscriptionService;
                updatedProtocol.PersonMadeRequest = protocolModel.PersonMadeRequest;

                if (!string.IsNullOrEmpty(protocolModel.RequestDate) && protocolModel.RequestDate != expectedDateFormat)
                {
                    updatedProtocol.RequestDate = DateTime.Parse(protocolModel.RequestDate);
                }
                else
                {
                    updatedProtocol.RequestDate = null;
                }
                updatedProtocol.HasCustomerProtocol = protocolModel.HasCustomerProtocol;
                updatedProtocol.InvoiceNumber = protocolModel.InvoiceNumber;
                if (!string.IsNullOrEmpty(protocolModel.InvoiceDate) && protocolModel.InvoiceDate != expectedDateFormat)
                {
                    updatedProtocol.InvoiceDate = DateTime.Parse(protocolModel.InvoiceDate);
                }
                else
                {
                    updatedProtocol.InvoiceDate = null;
                }
                updatedProtocol.Other = protocolModel.Other;
                updatedProtocol.WarrantyCardNumber = protocolModel.WarrantyCardNumber;
                updatedProtocol.WorkInHours = protocolModel.WorkInHours;
                updatedProtocol.PricePerHour = protocolModel.PricePerHour;
                updatedProtocol.PriceForChangedEguipment = protocolModel.PriceForChangedEguipment;
                updatedProtocol.DistanceInKm = protocolModel.DistanceInKm;
                updatedProtocol.PricePerKm = protocolModel.PricePerKm;

                updatedProtocol.CustomerId = protocolModel.CustomerId;
                updatedProtocol.UserId = protocolModel.UserId;
                updatedProtocol.Note = protocolModel.Note;


                this.protocols.Update(updatedProtocol);
                this.protocols.SaveChanges();

                // Edit installed equipment
                var oldInstalledEquipment = this.changedEquipment.All().Where(i => i.ProtocolId == protocolModel.Id).ToList();

                foreach (var item in oldInstalledEquipment)
                {
                    this.changedEquipment.Delete(item);
                }

                if (protocolModel.ChangedEquipment != null)
                {
                    foreach (var item in protocolModel.ChangedEquipment)
                    {
                        ChangedEquipment ie = new ChangedEquipment();
                        ie.ProtocolId = protocolModel.Id;
                        ie.EquipmentId = item.Id;
                        ie.OldSerialNumber = item.OldSerialNumber;
                        ie.NewSerialNumber = item.NewSerialNumber;
                        ie.Quantity = item.Quantity;
                        ie.EquipmentLength = item.EquipmentLength;
                        this.changedEquipment.Add(ie);
                    }
                }

                this.changedEquipment.SaveChanges();

                TempData["message"] = "Протоколът беше редактиран";

                return RedirectToAction("Index");
            }

            return View(protocolModel);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var protocol = this.protocols.All().Where(c => c.Id == id).Project().To<DetailsProtocolViewModel>().FirstOrDefault();

            if (protocol == null)
            {
                return HttpNotFound();
            }

            protocol.ProtocolDate = ConvertDbDate(protocol.ProtocolDate);
            if (protocol.InvoiceDate != string.Empty)
            {
                protocol.InvoiceDate = ConvertDbDate(protocol.InvoiceDate);
            }
            if (protocol.RequestDate != string.Empty)
            {
                protocol.RequestDate = ConvertDbDate(protocol.RequestDate);
            }

            protocol.ChangedEquipment = this.changedEquipment.All().Where(i => i.ProtocolId == protocol.Id).Project().To<ChangedEquipmentListViewModel>().ToList();

            var customerName = this.customers.All().Where(c => c.Id == protocol.CustomerId).FirstOrDefault();
            protocol.CustomerName = customerName.Name;

            var userName = this.users.All().Where(c => c.Id == protocol.UserId).FirstOrDefault();
            protocol.UserName = userName.FullName;

            ViewBag.ResultLabor = String.Format("{0:0.00}", protocol.WorkInHours * protocol.PricePerHour);
            ViewBag.ResultEquipment = String.Format("{0:0.00}", protocol.PriceForChangedEguipment);
            ViewBag.ResultDistance = String.Format("{0:0.00}", protocol.DistanceInKm * protocol.PricePerKm);

            return View(protocol);
        }

        private string ConvertDbDate(string date)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(date.Substring(8, 2));
            sb.Append("/");
            sb.Append(date.Substring(5, 2));
            sb.Append("/");
            sb.Append(date.Substring(0, 4));

            return sb.ToString();
        }
    }
}