namespace CastService.Web.Controllers
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

    public class InstallationsController : Controller
    {
        private readonly IDeletableEntityRepository<Installation> installations;
        private readonly IDeletableEntityRepository<Customer> customers;
        private readonly IDeletableEntityRepository<InstallatedEquipment> installedEquipment;

        public InstallationsController(
            IDeletableEntityRepository<Installation> installations, 
            IDeletableEntityRepository<Customer> customers,
            IDeletableEntityRepository<InstallatedEquipment> installedEquipment)
        {
            this.installations = installations;
            this.customers = customers;
            this.installedEquipment = installedEquipment;
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


        public ActionResult Create(DetailsInstallationViewModel installation)
        {
            if (ModelState.IsValid)
            {

                //Installation newInstallation = new Installation();

                //this.customers.SaveChanges();

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
            installation.InstalledEquipment = this.installedEquipment.All().Where(i => i.InstallationId == installation.Id).ToList();

            return View(installation);
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