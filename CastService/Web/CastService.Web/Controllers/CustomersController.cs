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
    using CastService.Web.ViewModels.Customers;

    [Authorize]
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

        [Authorize(Roles = "Администратор,Редактор")]
        public ActionResult Create()
        {
            var createCustomerViewModel = new CreateCustomerViewModel();

            createCustomerViewModel.CustomersNames = PopulateCustomers();

            return View(createCustomerViewModel);
        }

        [Authorize(Roles = "Администратор,Редактор")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateCustomerViewModel customer)
        {
            if (ModelState.IsValid)
            {
                var checkedCustomer = this.customers.All().Where(c => c.Name == customer.Name).FirstOrDefault();

                if (checkedCustomer != null)
                {
                    TempData["message"] = "Клиент с това име вече съществува"; 
                    return RedirectToAction("Index");
                }

                Customer newCustomer = new Customer();
                newCustomer.Name = customer.Name;
                newCustomer.Place = customer.Place;
                newCustomer.Eik = customer.Eik;
                newCustomer.Note = customer.Note;
                newCustomer.Address = customer.Address;
                newCustomer.OldNameId = customer.OldNameId;

                this.customers.Add(newCustomer);
                this.customers.SaveChanges();

                return RedirectToAction("Index");
            }

            customer.CustomersNames = PopulateCustomers();

            return View(customer);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var customer = this.customers.All().Where(c => c.Id == id).Project().To<DetailsCustomerViewModel>().FirstOrDefault();

            if (customer == null)
            {
                return HttpNotFound();
            }

            if (customer.OldNameId != 0)
            {
                var oldCustomerName = this.customers.All().Where(c => c.Id == customer.OldNameId).FirstOrDefault();
                if (oldCustomerName != null)
                {
                    customer.OldName = oldCustomerName.Name;
                }
            }

            return View(customer);
        }

        [Authorize(Roles = "Администратор,Редактор")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var customer = this.customers.All().Where(c => c.Id == id).Project().To<DetailsCustomerViewModel>().FirstOrDefault();

            if (customer == null)
            {
                return HttpNotFound();
            }

            customer.CustomersNames = PopulateCustomers(customer.OldNameId.GetValueOrDefault());

            return View(customer);
        }

        [Authorize(Roles = "Администратор,Редактор")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DetailsCustomerViewModel customer)
        {
            if (ModelState.IsValid)
            {
                var newCustomer = this.customers.All().Where(c => c.Id == customer.Id).FirstOrDefault();

                if (newCustomer == null)
                {
                    return HttpNotFound();
                }

                newCustomer.Name = customer.Name;
                newCustomer.Place = customer.Place;
                newCustomer.Eik = customer.Eik;
                newCustomer.Note = customer.Note;
                newCustomer.Address = customer.Address;
                newCustomer.OldNameId = customer.OldNameId;


                this.customers.Update(newCustomer);
                this.customers.SaveChanges();

                TempData["message"] = "Клиенът беше редактиран";
 
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        private IList<SelectListItem> PopulateCustomers(int selectedId = 0)
        {
            IList<SelectListItem> customersNames = this.customers.All().Select(c => new SelectListItem
                       {
                           Value = c.Id.ToString(),
                           Text = c.Name
                       })
                       .ToList();

            customersNames.Add(new SelectListItem {
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