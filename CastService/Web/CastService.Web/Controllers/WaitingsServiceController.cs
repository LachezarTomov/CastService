namespace CastService.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using CastService.Data.Common.Repository;
    using CastService.Data.Models;
    using CastService.Web.ViewModels.WaitingsService;
    using CastService.Web.Infrastructure.Populators;

    [Authorize]
    public class WaitingsServiceController : Controller
    {
        private readonly DropDownListPopulator populator;
        private readonly IDeletableEntityRepository<WaitingService> waitingService;

        public WaitingsServiceController(DropDownListPopulator populator, IDeletableEntityRepository<WaitingService> waitingService)
        {
            this.populator = populator;
            this.waitingService = waitingService;
        }

        // GET: Waitings
        public ActionResult Index()
        {
            var model = this.waitingService.All().OrderByDescending(d => d.PlannedDate).Project().To<ListWaitingsServiceViewModel>();

            return View(model);
        }

        [Authorize(Roles = "Администратор,Редактор")]
        public ActionResult Create()
        {
            var waitingViewModel = new DetailsWaitingServiceViewModel();
            waitingViewModel.CustomersNames = this.populator.PopulateCustomers();
            waitingViewModel.PlannedSpecialist = this.populator.PopulateUsers();

            return View(waitingViewModel);
        }

        [Authorize(Roles = "Администратор,Редактор")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DetailsWaitingServiceViewModel waitingService)
        {
            if (ModelState.IsValid)
            {
                var newWaitingService = new WaitingService();

                newWaitingService.ObjectNumber = waitingService.ObjectNumber;

                newWaitingService.RequestDate = DateTime.Parse(waitingService.RequestDate);
                newWaitingService.PlannedDate = DateTime.Parse(waitingService.PlannedDate);
                newWaitingService.CustomerId = waitingService.CustomerId;
                newWaitingService.UserId = waitingService.UserId;
                newWaitingService.SubmittedInfo = waitingService.SubmittedInfo;
                newWaitingService.ProblemDescription = waitingService.ProblemDescription;

                this.waitingService.Add(newWaitingService);
                this.waitingService.SaveChanges();

                TempData["message"] = "Нов автомобил, чакащ сервиз, беше създаден";

                return RedirectToAction("Index");
            }

            waitingService.CustomersNames = this.populator.PopulateCustomers();
            waitingService.PlannedSpecialist = this.populator.PopulateUsers();

            return View(waitingService);
        }
    }
}