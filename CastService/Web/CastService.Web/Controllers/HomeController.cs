namespace CastService.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using CastService.Data.Common.Repository;
    using CastService.Data.Models;

    public class HomeController : Controller
    {
        private IRepository<Customer> customers;
        public HomeController(IRepository<Customer> customers)
        {
            this.customers = customers;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}