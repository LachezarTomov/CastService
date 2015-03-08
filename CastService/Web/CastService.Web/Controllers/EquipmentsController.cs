namespace CastService.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Net;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using CastService.Data.Common.Repository;
    using CastService.Data.Models;
    using CastService.Web.ViewModels.Equipments;

    [Authorize]
    public class EquipmentsController : Controller
    {
        private readonly IDeletableEntityRepository<Equipment> equipments;

        public EquipmentsController(IDeletableEntityRepository<Equipment> equipments)
        {
            this.equipments = equipments;
        }


        // GET: Equipments
        public ActionResult Index()
        {
            var model = this.equipments.All().Project().To<ListEquipmentsViewModel>().ToList();
            return View(model);
        }

        public ActionResult Create()
        {
            var createCustomerViewModel = new DetailEquipmentViewModel();

            return View(createCustomerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DetailEquipmentViewModel equipment)
        {
            if (ModelState.IsValid)
            {
                var checkedCustomer = this.equipments.All().Where(c => c.Name == equipment.Name).FirstOrDefault();

                if (checkedCustomer != null)
                {
                    TempData["message"] = "Артикул с това име вече съществува";
                    return RedirectToAction("Index");
                }

                Equipment newEquipment = new Equipment();
                newEquipment.Name = equipment.Name;
                newEquipment.Model = equipment.Model;
                
                this.equipments.Add(newEquipment);
                this.equipments.SaveChanges();

                return RedirectToAction("Index");
            }

            TempData["message"] = "Новият артикул е създаден";

            return View(equipment);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var customer = this.equipments.All().Where(c => c.Id == id).Project().To<DetailEquipmentViewModel>().FirstOrDefault();

            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var customer = this.equipments.All().Where(c => c.Id == id).Project().To<DetailEquipmentViewModel>().FirstOrDefault();

            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DetailEquipmentViewModel equipment)
        {
            if (ModelState.IsValid)
            {
                var newEquipment = this.equipments.All().Where(c => c.Id == equipment.Id).FirstOrDefault();

                if (newEquipment == null)
                {
                    return HttpNotFound();
                }

                newEquipment.Name = equipment.Name;
                newEquipment.Model = equipment.Model;

                this.equipments.Update(newEquipment);
                this.equipments.SaveChanges();

                TempData["message"] = "Артикулът беше редактиран";

                return RedirectToAction("Index");
            }

            return View(equipment);
        }
    }
}