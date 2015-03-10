namespace CastService.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Net;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using CastService.Data;
    using CastService.Data.Common.Repository;
    using CastService.Data.Models;
    using CastService.Web.ViewModels.Users;
    using System.Web.Security;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Threading.Tasks;

    [Authorize(Roles = "Администратор")]
    public class UsersController : Controller
    {
        private readonly IDeletableEntityRepository<User> users;

        public UsersController(IDeletableEntityRepository<User> users)
        {
            this.users = users;
        }

        // GET: Users
        [Authorize(Roles = "Администратор")]
        public async Task<ActionResult> Index()
        {
            CastServiceDbContext db = new CastServiceDbContext();

            var model = this.users.All().Project().To<ListUsersViewModel>().ToList();
            
            foreach (var item in model)
            {
                using (var userManager = new UserManager<User>(new UserStore<User>(db)))
                {
                    var rolesForUser = await userManager.GetRolesAsync(item.Id);

                    if (rolesForUser.Count > 0)
                    {
                       item.Role = rolesForUser[0];
                    }
                }
            }
            db.Dispose();
            return View(model);
        }

        private SelectList GetRolesList(string selectedId = "0")
        {
            CastServiceDbContext db = new CastServiceDbContext();
            var groups = db.Roles.OrderByDescending(r => r.Name).ToList();
            var list = new List<SelectListItem>();
            foreach (var group in groups)
            {
                list.Add(new SelectListItem()
                {
                    Value = group.Id,
                    Text = group.Name
                });
            }

            if (selectedId != "0")
            {
                foreach (var item in list)
                {
                    if (item.Value == selectedId.ToString())
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
            db.Dispose();
            return new SelectList(list, "Value", "Text");
        }
    }
}