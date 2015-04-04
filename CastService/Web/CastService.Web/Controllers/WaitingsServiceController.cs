namespace CastService.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;
    using PagedList;

    using CastService.Data.Common.Repository;
    using CastService.Data.Models;
    using CastService.Web.ViewModels.WaitingsService;
    using CastService.Web.Infrastructure.Populators;
    using System.Net;
    using System.Text;

    [Authorize]
    public class WaitingsServiceController : Controller
    {
        private readonly DropDownListPopulator populator;
        private readonly IDeletableEntityRepository<WaitingService> waitingService;
        private readonly IDeletableEntityRepository<User> users;

        public WaitingsServiceController(DropDownListPopulator populator, 
            IDeletableEntityRepository<WaitingService> waitingService,
            IDeletableEntityRepository<User> users)
        {
            this.populator = populator;
            this.waitingService = waitingService;
            this.users = users;
        }

        // GET: Waitings
        public ActionResult Index(string sortOrder, int? page)
        {
            var waitingsViewModel = this.waitingService.All()
                .OrderByDescending(d => d.PlannedDate)
                .Project()
                .To<ListWaitingsServiceViewModel>().ToList();

            foreach (var item in waitingsViewModel)
            {
                var userFullName = this.users.All().Where(u => u.Id == item.UserId).Select(u => u.FullName).FirstOrDefault();
                item.PlannedSpecialist = userFullName;
            }

            ViewBag.ObjectNumberSortParams = sortOrder == "objectNumber" ? "objectNumberDesc" : "objectNumber";
            ViewBag.CustomerNameSortParams = sortOrder == "customerName" ? "customerNameDesc" : "customerName";
            ViewBag.RequestDateSortParams = sortOrder == "requestDate" ? "requestDateDesc" : "requestDate";
            ViewBag.PlannedDateSortParams = sortOrder == "plannedDate" ? "plannedDateDesc" : "plannedDate";
            ViewBag.PlannedSpecialistSortParams = sortOrder == "plannedSpecialist" ? "plannedSpecialistDesc" : "plannedSpecialist";
            ViewBag.IsDoneSortParams = sortOrder == "isDone" ? "isDoneDesc" : "isDone";

            switch (sortOrder)
            {
                case "objectNumber":
                    waitingsViewModel = waitingsViewModel.OrderBy(o => o.ObjectNumber).ToList();
                    break;
                case "objectNumberDesc":
                    waitingsViewModel = waitingsViewModel.OrderByDescending(o => o.ObjectNumber).ToList();
                    break;
                case "customerName":
                    waitingsViewModel = waitingsViewModel.OrderBy(o => o.CustomerName).ToList();
                    break;
                case "customerNameDesc":
                    waitingsViewModel = waitingsViewModel.OrderByDescending(o => o.CustomerName).ToList();
                    break;
                case "requestDate":
                    waitingsViewModel = waitingsViewModel.OrderBy(o => o.RequestDate).ToList();
                    break;
                case "requestDateDesc":
                    waitingsViewModel = waitingsViewModel.OrderByDescending(o => o.RequestDate).ToList();
                    break;
                case "plannedDate":
                    waitingsViewModel = waitingsViewModel.OrderBy(o => o.PlannedDate).ToList();
                    break;
                case "plannedDateDesc":
                    waitingsViewModel = waitingsViewModel.OrderByDescending(o => o.PlannedDate).ToList();
                    break;
                case "plannedSpecialist":
                    waitingsViewModel = waitingsViewModel.OrderBy(o => o.PlannedSpecialist).ToList();
                    break;
                case "plannedSpecialistDesc":
                    waitingsViewModel = waitingsViewModel.OrderByDescending(o => o.PlannedSpecialist).ToList();
                    break;
                case "isDone":
                    waitingsViewModel = waitingsViewModel.OrderBy(o => o.IsDone).ToList();
                    break;
                case "isDoneDesc":
                    waitingsViewModel = waitingsViewModel.OrderByDescending(o => o.IsDone).ToList();
                    break;

                default:
                    waitingsViewModel = waitingsViewModel.OrderByDescending(o => o.RequestDate).ToList();
                    break;
            }

            int pageSize = 15;
            int pageNumber = (page ?? 1);

            return View(waitingsViewModel.ToPagedList(pageNumber, pageSize));
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

                TempData["message"] = "Нова заявака за сервиз, беше създадена";

                return RedirectToAction("Index");
            }

            waitingService.CustomersNames = this.populator.PopulateCustomers();
            waitingService.PlannedSpecialist = this.populator.PopulateUsers();

            return View(waitingService);
        }

        [Authorize(Roles = "Администратор,Редактор")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var editedWaitingService = this.waitingService.All().Where(c => c.Id == id).Project().To<DetailsWaitingServiceViewModel>().FirstOrDefault();

            if (editedWaitingService == null)
            {
                return HttpNotFound();
            }

            editedWaitingService.RequestDate = ConvertDbDate(editedWaitingService.RequestDate);
            editedWaitingService.PlannedDate = ConvertDbDate(editedWaitingService.PlannedDate);
            editedWaitingService.CustomersNames = this.populator.PopulateCustomers(editedWaitingService.CustomerId);
            editedWaitingService.PlannedSpecialist = this.populator.PopulateUsers(editedWaitingService.UserId);

            return View(editedWaitingService);
        }

        [Authorize(Roles = "Администратор,Редактор")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DetailsWaitingServiceViewModel waitingServiceModel)
        {
            if (ModelState.IsValid)
            {
                var updatedInstallation = this.waitingService.All().Where(c => c.Id == waitingServiceModel.Id).FirstOrDefault();

                if (updatedInstallation == null)
                {
                    return HttpNotFound();
                }

                updatedInstallation.ObjectNumber = waitingServiceModel.ObjectNumber;

                updatedInstallation.RequestDate = DateTime.Parse(waitingServiceModel.RequestDate);
                updatedInstallation.PlannedDate = DateTime.Parse(waitingServiceModel.PlannedDate);
                updatedInstallation.CustomerId = waitingServiceModel.CustomerId;
                updatedInstallation.UserId = waitingServiceModel.UserId;
                updatedInstallation.ProblemDescription = waitingServiceModel.ProblemDescription;
                updatedInstallation.SubmittedInfo = waitingServiceModel.SubmittedInfo;

                if (waitingServiceModel.IsDone)
                {
                    updatedInstallation.IsDone = waitingServiceModel.IsDone;
                }

                this.waitingService.Update(updatedInstallation);
                this.waitingService.SaveChanges();


                TempData["message"] = "Заявката за сервиз, беше редактирана";

                return RedirectToAction("Index");
            }

            return View(waitingServiceModel);
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