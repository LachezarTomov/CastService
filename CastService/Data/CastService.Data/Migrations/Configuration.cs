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
                new ServiceType { Description = "�����������" },
                new ServiceType { Description = "����������������" },
                new ServiceType { Description = "�����������" },
                new ServiceType { Description = "�� ������" }
            );

            //ServiceType serviceType = new ServiceType()
            //{
            //    Description = "�����������"
            //};
            //context.ServiceTypes.Add(serviceType);

            //serviceType = new ServiceType()
            //{
            //    Description = "����������������"
            //};
            //context.ServiceTypes.Add(serviceType);

            //serviceType = new ServiceType()
            //{
            //    Description = "�����������"
            //};
            //context.ServiceTypes.Add(serviceType);

            //serviceType = new ServiceType()
            //{
            //    Description = "�� ������"
            //};
            //context.ServiceTypes.Add(serviceType);

            context.SaveChanges();

        }
    }
}
