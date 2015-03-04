namespace CastService.Data.Migrations
{
   
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using CastService.Data.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public sealed class Configuration : DbMigrationsConfiguration<CastServiceDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            // TODO: remove in production
            this.AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(CastServiceDbContext context)
        {
            this.SeedServiceTypes(context);
        }

        private void SeedServiceTypes(CastServiceDbContext context)
        {
            //if (context.Users.Any())
            //{
            //    return;
            //}

            const string AdminRole = "Администратор";
            const string ServiceRole = "Сервизен инженер";
            const string WriterRole = "Редактор";

            var userManager = new UserManager<User>(new UserStore<User>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            roleManager.Create(new IdentityRole(AdminRole));
            roleManager.Create(new IdentityRole(ServiceRole));
            roleManager.Create(new IdentityRole(WriterRole));

            var admin = new User
            {
                UserName = "lachezar",
                FullName = "Лъчезар Томов"
            };
            var admin2 = new User
            {
                UserName = "daniel",
                FullName = "Даниел Китин"
            };
            var userB = new User
            {
                UserName = "test1",
                FullName = "Тест 1"
            };

            var userC = new User
            {
                UserName = "test2",
                FullName = "Тест 2"
            };

            var resultAdmin = userManager.Create(admin, "123456");
            if (resultAdmin.Succeeded)
            {
                userManager.AddToRole(admin.Id, AdminRole);
            }
            resultAdmin = userManager.Create(admin2, "123456");
            if (resultAdmin.Succeeded)
            {
                userManager.AddToRole(admin2.Id, AdminRole);
            }
            resultAdmin = userManager.Create(userB, "123456");
            if (resultAdmin.Succeeded)
            {
                userManager.AddToRole(userB.Id, ServiceRole);
            }
            resultAdmin = userManager.Create(userC, "123456");
            if (resultAdmin.Succeeded)
            {
                userManager.AddToRole(userC.Id, WriterRole);
            }

            context.SaveChanges();
        }
    }
}
