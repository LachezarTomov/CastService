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
    using CastService.Web.ViewModels;
    using CastService.Web.ViewModels.Customers;

    public class CustomersController : Controller
    {
        private readonly IDeletableEntityRepository<Customer> customers;

        public CustomersController(IDeletableEntityRepository<Customer> customers)
        {
            this.customers = customers;
        }

        // GET: Customers
        public ActionResult Index()
        {
            var model = this.customers.All().Project().To<ListCustomersViewModel>().ToList();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateCustomerViewModel customer)
        {
            if (ModelState.IsValid)
            {
                Customer newCustomer = new Customer();
                newCustomer.Name = customer.Name;
                newCustomer.Place = customer.Place;
                newCustomer.Eik = customer.Eik;
                newCustomer.Note = customer.Note;

                this.customers.Add(newCustomer);
                this.customers.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(customer);
        }

    }
}