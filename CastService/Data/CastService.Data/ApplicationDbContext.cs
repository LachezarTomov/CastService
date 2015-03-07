namespace CastService.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Microsoft.AspNet.Identity.EntityFramework;

    using CastService.Data.Common.Models;
    using CastService.Data.Models;
    using CastService.Data.Migrations;
    
    public class CastServiceDbContext : IdentityDbContext<User>
    {
        public CastServiceDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CastServiceDbContext, Configuration>());
        }

        public static CastServiceDbContext Create()
        {
            return new CastServiceDbContext();
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<ServiceType> ServiceTypes { get; set; }

        public DbSet<Installation> Installations { get; set; }

        public DbSet<Equipment> Equipments { get; set; }

        public DbSet<Protocol> Protocols { get; set; }



        public override int SaveChanges()
        {
            this.ApplyAuditInfoRules();
            this.ApplyDeletableEntityRules();
            return base.SaveChanges();
        }

        private void ApplyAuditInfoRules()
        {
            // Approach via @julielerman: http://bit.ly/123661P
            foreach (var entry in
                this.ChangeTracker.Entries()
                    .Where(
                        e =>
                        e.Entity is IAuditInfo && ((e.State == EntityState.Added) || (e.State == EntityState.Modified))))
            {
                var entity = (IAuditInfo)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    if (!entity.PreserveCreatedOn)
                    {
                        entity.CreatedOn = DateTime.Now;
                    }
                }
                else
                {
                    entity.ModifiedOn = DateTime.Now;
                }
            }
        }

        private void ApplyDeletableEntityRules()
        {
            // Approach via @julielerman: http://bit.ly/123661P
            foreach (
                var entry in
                    this.ChangeTracker.Entries()
                        .Where(e => e.Entity is IDeletableEntity && (e.State == EntityState.Deleted)))
            {
                var entity = (IDeletableEntity)entry.Entity;

                entity.DeletedOn = DateTime.Now;
                entity.IsDeleted = true;
                entry.State = EntityState.Modified;
            }
        }

 //       public System.Data.Entity.DbSet<CastService.Web.ViewModels.Users.DetailsUserViewModel> DetailsUserViewModels { get; set; }
    }
}
