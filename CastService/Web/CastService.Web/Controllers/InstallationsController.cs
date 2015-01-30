﻿namespace CastService.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using CastService.Data.Common.Repository;
    using CastService.Data.Models;
    using CastService.Web.ViewModels.Installations;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;

    public class InstallationsController : Controller
    {
        private readonly IDeletableEntityRepository<Installation> installations;
        private readonly IDeletableEntityRepository<Customer> customers;
        private readonly IDeletableEntityRepository<InstalledEquipment> installedEquipment;
        private readonly IDeletableEntityRepository<Equipment> equipments;

        public InstallationsController(
            IDeletableEntityRepository<Installation> installations,
            IDeletableEntityRepository<Customer> customers,
            IDeletableEntityRepository<Equipment> equipments,
            IDeletableEntityRepository<InstalledEquipment> installedEquipment)
        {
            this.installations = installations;
            this.customers = customers;
            this.installedEquipment = installedEquipment;
            this.equipments = equipments;
        }

        // GET: Installations
        public ActionResult Index()
        {
            var model = this.installations.All().Project().To<ListInstallationsViewModel>().ToList();
            return View(model);
        }

        public ActionResult Create()
        {
            var installationViewModel = new DetailsInstallationViewModel();
            installationViewModel.CustomersNames = PopulateCustomers();

            return View(installationViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DetailsInstallationViewModel installation)
        {
            if (ModelState.IsValid)
            {
                var newInstallation = new Installation();

                newInstallation.ObjectName = installation.ObjectName;
                newInstallation.InstallationDate = DateTime.Parse(installation.InstallationDate);
                newInstallation.StartTime = installation.StartTime;
                newInstallation.EndTime = installation.EndTime;
                newInstallation.GuessedTime = installation.GuessedTime;
                newInstallation.DetectedFaults = installation.DetectedFaults;
                newInstallation.AdditionalActivities = installation.AdditionalActivities;
                newInstallation.CustomerId = installation.CustomerId;
                newInstallation.Note = installation.Note;

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

                if (installation.InstalledEquipment == null)
                {
                    foreach (var item in installation.InstalledEquipment)
                    {
                        InstalledEquipment ie = new InstalledEquipment();
                        ie.InstallationId = installation.Id;
                        ie.EquipmentId = item.Id;
                        ie.SerialNumber = item.SerialNumber;
                        ie.Quantity = item.Quantity;
                        this.installedEquipment.Add(ie);
                    }

                    this.installedEquipment.SaveChanges();
                }

                TempData["message"] = "Инсталацията беше редактирана";

                return RedirectToAction("Index");
            }

            installation.CustomersNames = PopulateCustomers();

            return View(installation);
        }

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

            installation.CustomersNames = PopulateCustomers(installation.CustomerId);
            installation.InstallationDate = ConvertDbDate(installation.InstallationDate);
            if (!string.IsNullOrEmpty(installation.InvoiceDate))
            {
                installation.InvoiceDate = ConvertDbDate(installation.InvoiceDate);
            }

            installation.InstalledEquipment = this.installedEquipment.All().Where(i => i.InstallationId == installation.Id).Project().To<InstalledEquipmentListViewModel>().ToList();

            return View(installation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DetailsInstallationViewModel installation)
        {
            if (ModelState.IsValid)
            {
                var updatedInstallation = this.installations.All().Where(c => c.Id == installation.Id).FirstOrDefault();

                if (updatedInstallation == null)
                {
                    return HttpNotFound();
                }

                updatedInstallation.ObjectName = installation.ObjectName;
                updatedInstallation.InstallationDate = DateTime.Parse(installation.InstallationDate);
                updatedInstallation.StartTime = installation.StartTime;
                updatedInstallation.EndTime = installation.EndTime;
                updatedInstallation.GuessedTime = installation.GuessedTime;
                updatedInstallation.DetectedFaults = installation.DetectedFaults;
                updatedInstallation.AdditionalActivities = installation.AdditionalActivities;
                updatedInstallation.CustomerId = installation.CustomerId;
                updatedInstallation.Note = installation.Note;

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
                
                foreach (var item in installation.InstalledEquipment)
                {
                    InstalledEquipment ie = new InstalledEquipment();
                    ie.InstallationId = installation.Id;
                    ie.EquipmentId = item.Id;
                    ie.SerialNumber = item.SerialNumber;
                    ie.Quantity = item.Quantity;
                    this.installedEquipment.Add(ie);
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

        private IList<SelectListItem> PopulateCustomers(int selectedId = 0)
        {
            IList<SelectListItem> customersNames = this.customers.All().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            customersNames.Add(new SelectListItem
            {
                Value = "0",
                Text = ""
            });

            foreach (var item in customersNames)
            {
                if (item.Value == selectedId.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }

            return customersNames;
        }
    }
}