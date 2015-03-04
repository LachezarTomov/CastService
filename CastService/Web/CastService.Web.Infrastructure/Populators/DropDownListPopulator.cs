namespace CastService.Web.Infrastructure.Populators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using CastService.Data.Common.Repository;
    using CastService.Data.Models;
    using CastService.Data;

    public class DropDownListPopulator
    {
        private readonly IDeletableEntityRepository<Customer> customers;
        private readonly IDeletableEntityRepository<User> users;

        public DropDownListPopulator(IDeletableEntityRepository<Customer> customers,
            IDeletableEntityRepository<User> users
            )
        {
            this.customers = customers;
            this.users = users;
        }

        public IList<SelectListItem> PopulateUsers(string selectedId = "0")
        {
            IList<SelectListItem> usersNames = this.users.All().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.FullName
            }).ToList();

            usersNames.Add(new SelectListItem
            {
                Value = "0",
                Text = ""
            });

            foreach (var item in usersNames)
            {
                if (item.Value == selectedId.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }

            return usersNames;
        }

        public IList<SelectListItem> PopulateCustomers(int selectedId = 0)
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
