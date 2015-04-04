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
    using CastService.Web.ViewModels.Installations;
    using CastService.Web.Infrastructure.Populators;
    
    [Authorize]
    public class InstallationsController : Controller
    {
        private readonly IDeletableEntityRepository<Installation> installations;
        private readonly IDeletableEntityRepository<Customer> customers;
        private readonly IDeletableEntityRepository<InstalledEquipment> installedEquipment;
        private readonly IDeletableEntityRepository<User> users;
        private readonly IDeletableEntityRepository<Equipment> equipments;
        private readonly DropDownListPopulator populator;

        public InstallationsController(
            IDeletableEntityRepository<Installation> installations,
            IDeletableEntityRepository<Customer> customers,
            IDeletableEntityRepository<Equipment> equipments,
            IDeletableEntityRepository<User> users,
            IDeletableEntityRepository<InstalledEquipment> installedEquipment,
            DropDownListPopulator populator)
        {
            this.installations = installations;
            this.customers = customers;
            this.installedEquipment = installedEquipment;
            this.users = users;
            this.equipments = equipments;
            this.populator = populator;
        }

        // GET: Installations
        public ActionResult Index(string sortOrder,  string currentFilter, int? page, string searchByCustomerName, string searchByInstalledPartSerNum)
        {
            var model = this.installations.All().Project().To<ListInstallationsViewModel>();

            ViewBag.ClientNameSortParams = sortOrder == "clientName" ? "clientNameDesc" : "clientName";
            ViewBag.PlaceSortParams = sortOrder == "place" ? "placeDesc" : "place";
            ViewBag.DateSortParams = string.IsNullOrEmpty(sortOrder) ? "date" : "";

            if (searchByCustomerName != null)
            {
                page = 1;
            }
            else
            {
                searchByCustomerName = currentFilter;
            }

            if (!string.IsNullOrEmpty(searchByCustomerName))
            {
                model = model.Where(i => i.CustomerName.ToLower().Contains(searchByCustomerName.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchByInstalledPartSerNum))
            {
                model = model.Where(i => i.InstallatedEquipment.Any(
                    c => c.SerialNumber.ToLower().Contains(searchByInstalledPartSerNum.ToLower())
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
                case "place":
                    model = model.OrderBy(o => o.ObjectName);
                    break;
                case "placeDesc":
                    model = model.OrderByDescending(o => o.ObjectName);
                    break;
                case "date":
                    model = model.OrderBy(o => o.InstallationDate);
                    break;
                default:
                    model = model.OrderByDescending(o => o.InstallationDate);
                    break;
            }

            int pageSize = 15;
            int pageNumber = (page ?? 1);

            //return View(model.ToList());
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "Администратор,Редактор")]
        public ActionResult Create()
        {
            var installationViewModel = new DetailsInstallationViewModel();
            //installationViewModel.CustomersNames = PopulateCustomers();
            installationViewModel.CustomersNames = this.populator.PopulateCustomers();
            installationViewModel.UserNames = this.populator.PopulateUsers();

            return View(installationViewModel);
        }

        [Authorize(Roles = "Администратор,Редактор")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DetailsInstallationViewModel installation)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Parse(installation.InstallationDate);
                var duplicateInstallationCheck = this.installations.All()
                    .Where(c => c.CustomerId == installation.CustomerId)
                    .Where(c => c.InstallationDate == dt)
                    .Where(c => c.StartTime == installation.StartTime)
                    .FirstOrDefault();

                if (duplicateInstallationCheck != null)
                {
                    installation.CustomersNames = this.populator.PopulateCustomers();
                    installation.UserNames = this.populator.PopulateUsers();
                    TempData["message"] = "Инсталация за този клиент със същата дата и час вече съществува";

                    return View(installation);
                }

                var newInstallation = new Installation();

                newInstallation.ObjectName = installation.ObjectName;
                newInstallation.ObjectType = installation.ObjectType;
                newInstallation.ObjectNumber = installation.ObjectNumber;

                newInstallation.InstallationDate = DateTime.Parse(installation.InstallationDate);
                newInstallation.StartTime = installation.StartTime;
                newInstallation.EndTime = installation.EndTime;
                newInstallation.GuessedTime = installation.GuessedTime;
                newInstallation.DetectedFaults = installation.DetectedFaults;
                newInstallation.AdditionalActivities = installation.AdditionalActivities;
                newInstallation.CustomerId = installation.CustomerId;
                newInstallation.Note = installation.Note;
                newInstallation.WarrantyCardNumber = installation.WarrantyCardNumber;
                newInstallation.Other = installation.Other;
                newInstallation.WorkInHours = installation.WorkInHours;
                newInstallation.PricePerHour = installation.PricePerHour;
                newInstallation.PriceForChangedEguipment = installation.PriceForChangedEguipment;

                newInstallation.UserId = installation.UserId;

                if (installation.HasProtocol)
                {
                    newInstallation.HasProtocol = installation.HasProtocol;
                }
                if (!string.IsNullOrEmpty(installation.InvoiceNumber))
                {
                    newInstallation.InvoiceNumber = installation.InvoiceNumber;
                }
                if (installation.InvoiceDate != null)
                {
                    newInstallation.InvoiceDate = DateTime.Parse(installation.InvoiceDate);
                }

                this.installations.Add(newInstallation);
                this.installations.SaveChanges();


                if (installation.InstalledEquipment != null)
                {
                    foreach (var item in installation.InstalledEquipment)
                    {
                        InstalledEquipment ie = new InstalledEquipment();
                        ie.InstallationId = newInstallation.Id;
                        ie.EquipmentId = item.Id;
                        ie.SerialNumber = item.SerialNumber;
                        ie.Quantity = item.Quantity;
                        ie.EquipmentLength = item.EquipmentLength;
                        this.installedEquipment.Add(ie);
                    }

                    this.installedEquipment.SaveChanges();
                }

                TempData["message"] = "Инсталацията беше създадена";

                return RedirectToAction("Index");
            }

            installation.CustomersNames = this.populator.PopulateCustomers();
            installation.UserNames = this.populator.PopulateUsers();

            return View(installation);
        }

        [Authorize(Roles = "Администратор,Редактор")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var installation = this.installations.All().Where(c => c.Id == id).Project().To<DetailsInstallationViewModel>().FirstOrDefault();

            if (installation == null)
            {
                return HttpNotFound();
            }

            installation.CustomersNames = this.populator.PopulateCustomers(installation.CustomerId);
            installation.UserNames = this.populator.PopulateUsers(installation.UserId);

            installation.InstallationDate = ConvertDbDate(installation.InstallationDate);
            if (!string.IsNullOrEmpty(installation.InvoiceDate))
            {
                installation.InvoiceDate = ConvertDbDate(installation.InvoiceDate);
            }

            installation.InstalledEquipment = this.installedEquipment.All().Where(i => i.InstallationId == installation.Id).Project().To<InstalledEquipmentListViewModel>().ToList();

            return View(installation);
        }

        [Authorize(Roles = "Администратор,Редактор")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DetailsInstallationViewModel installation)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Parse(installation.InstallationDate);
                var duplicateInstallationCheck = this.installations.All()
                    .Where(c => c.CustomerId == installation.CustomerId)
                    .Where(c => c.InstallationDate == dt)
                    .Where(c => c.StartTime == installation.StartTime)
                    .FirstOrDefault();

                if (duplicateInstallationCheck != null)
                {
                    installation.CustomersNames = this.populator.PopulateCustomers();
                    installation.UserNames = this.populator.PopulateUsers();
                    TempData["message"] = "Инсталация за този клиент със същата дата и час вече съществува";

                    return View(installation);
                }

                var updatedInstallation = this.installations.All().Where(c => c.Id == installation.Id).FirstOrDefault();

                if (updatedInstallation == null)
                {
                    return HttpNotFound();
                }

                updatedInstallation.ObjectNumber = installation.ObjectNumber;
                updatedInstallation.ObjectType = installation.ObjectType;
                updatedInstallation.ObjectName = installation.ObjectName;
                updatedInstallation.InstallationDate = DateTime.Parse(installation.InstallationDate);
                updatedInstallation.StartTime = installation.StartTime;
                updatedInstallation.EndTime = installation.EndTime;
                updatedInstallation.GuessedTime = installation.GuessedTime;
                updatedInstallation.DetectedFaults = installation.DetectedFaults;
                updatedInstallation.AdditionalActivities = installation.AdditionalActivities;
                updatedInstallation.CustomerId = installation.CustomerId;
                updatedInstallation.Note = installation.Note;
                updatedInstallation.WarrantyCardNumber = installation.WarrantyCardNumber;
                updatedInstallation.Other = installation.Other;
                updatedInstallation.WorkInHours = installation.WorkInHours;
                updatedInstallation.PricePerHour = installation.PricePerHour;
                updatedInstallation.PriceForChangedEguipment = installation.PriceForChangedEguipment;

                if (installation.HasProtocol) 
                { 
                    updatedInstallation.HasProtocol = installation.HasProtocol;
                }
                if (!string.IsNullOrEmpty(installation.InvoiceNumber))
                {
                    updatedInstallation.InvoiceNumber = installation.InvoiceNumber;
                }
                if (installation.InvoiceDate != null)
                {
                    updatedInstallation.InvoiceDate = DateTime.Parse(installation.InvoiceDate);
                }

                this.installations.Update(updatedInstallation);
                this.installations.SaveChanges();

                // Edit installed equipment
                var oldInstalledEquipment = this.installedEquipment.All().Where(i => i.InstallationId == installation.Id).ToList();

                foreach (var item in oldInstalledEquipment)
                {
                    this.installedEquipment.Delete(item);
                }

                if (installation.InstalledEquipment != null)
                {
                    foreach (var item in installation.InstalledEquipment)
                    {
                        InstalledEquipment ie = new InstalledEquipment();
                        ie.InstallationId = installation.Id;
                        ie.EquipmentId = item.Id;
                        ie.SerialNumber = item.SerialNumber;
                        ie.Quantity = item.Quantity;
                        ie.EquipmentLength = item.EquipmentLength;
                        this.installedEquipment.Add(ie);
                    }
                }
                this.installedEquipment.SaveChanges();

                TempData["message"] = "Инсталацията беше редактирана";

                return RedirectToAction("Index");
            }

            return View(installation);
        }

        public ActionResult GetEquipment(string term)
        {
            var result = this.equipments.All().Where(e => e.Name.ToLower().Contains(term.ToLower())).Select(v => new { label = v.Name, value = v.Id }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var installation = this.installations.All().Where(c => c.Id == id).Project().To<DetailsInstallationViewModel>().FirstOrDefault();

            if (installation == null)
            {
                return HttpNotFound();
            }

            installation.InstallationDate = ConvertDbDate(installation.InstallationDate);
            installation.InstalledEquipment = this.installedEquipment.All().Where(i => i.InstallationId == installation.Id).Project().To<InstalledEquipmentListViewModel>().ToList();

            var customerName = this.customers.All().Where(c => c.Id == installation.CustomerId).FirstOrDefault();
            installation.CustomerName = customerName.Name;

            var userName = this.users.All().Where(c => c.Id == installation.UserId).FirstOrDefault();
            installation.UserName = userName.FullName;

            return View(installation);
        }

        private string ConvertDbDate(string date)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(date.Substring(8, 2));
            sb.Append("/"); 
            sb.Append(date.Substring(5, 2));
            sb.Append("/");
            sb.Append(date.Substring(0,4));

            return sb.ToString();
        }

        private string ConvertWebDate(string date)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(date.Substring(0, 4));
            sb.Append("-");
            sb.Append(date.Substring(5, 2));
            sb.Append("-");
            sb.Append(date.Substring(8, 2));
            
            return sb.ToString();
        }

        private string ListEquipment(ICollection<InstalledEquipment> equipment)
        {
            StringBuilder result = new StringBuilder();
            foreach (var eq in equipment)
            {
                result.Append(eq.Equipment.Name);
                result.Append("(");
                result.Append(eq.Quantity);
                result.Append(")<br />");
            }

            return result.ToString();
        }

    }
}