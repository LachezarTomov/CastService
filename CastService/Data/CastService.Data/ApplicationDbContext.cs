using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;

using Microsoft.AspNet.Identity.EntityFramework;

using CastService.Data.Models;
using CastService.Data.Migrations;


namespace CastService.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
