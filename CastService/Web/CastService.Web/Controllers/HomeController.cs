namespace CastService.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;
    using PagedList;

    using CastService.Data.Common.Repository;
    using CastService.Data.Models;
    using CastService.Web.ViewModels.WaitingsService;

    public class HomeController : Controller
    {
        private IRepository<Customer> customers;
        private readonly IDeletableEntityRepository<WaitingService> waitingService;
        private readonly IDeletableEntityRepository<User> users;

        public HomeController(IRepository<Customer> customers, 
            IDeletableEntityRepository<WaitingService> waitingService,
            IDeletableEntityRepository<User> users)
        {
            this.customers = customers;
            this.users = users;
            this.waitingService = waitingService;
        }

        public ActionResult Index(string sortOrder, int? page)
        {
            var waitingsViewModel = this.waitingService.All()
                .Where(ws => ws.IsDone == false)
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
    }
}