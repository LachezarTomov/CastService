namespace CastService.Data.Migrations
{
   
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using CastService.Data.Models;

    public sealed class Configuration : DbMigrationsConfiguration<CastServiceDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            // TODO: remove in production
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(CastServiceDbContext context)
        {
            this.SeedServiceTypes(context);
        }

        private void SeedServiceTypes(CastServiceDbContext context)
        {
            if (context.ServiceTypes.Any())
            {
                return;
            }

            context.ServiceTypes.AddOrUpdate(
                new ServiceType { Description = "гаранционно" },
                new ServiceType { Description = "извънгаранционно" },
                new ServiceType { Description = "абонаментно" },
                new ServiceType { Description = "по заявка" }
            );

            //ServiceType serviceType = new ServiceType()
            //{
            //    Description = "гаранционно"
            //};
            //context.ServiceTypes.Add(serviceType);

            //serviceType = new ServiceType()
            //{
            //    Description = "извънгаранционно"
            //};
            //context.ServiceTypes.Add(serviceType);

            //serviceType = new ServiceType()
            //{
            //    Description = "абонаментно"
            //};
            //context.ServiceTypes.Add(serviceType);

            //serviceType = new ServiceType()
            //{
            //    Description = "по заявка"
            //};
            //context.ServiceTypes.Add(serviceType);

            context.SaveChanges();

        }
    }
}
