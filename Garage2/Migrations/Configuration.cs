namespace Garage2.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Garage2.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Garage2.Models.Garage2Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
           // AutomaticMigrationDataLossAllowed = false;
            ContextKey = "Garage2.Models.Garage2Context";
        }

        protected override void Seed(Garage2.Models.Garage2Context context)
        {

            context.VehicleTypes.AddOrUpdate(v => v.Type, new VehicleType { Type = "Car" },
                                                       new VehicleType { Type = "Bus" },
                                                       new VehicleType { Type = "Boat" },
                                                       new VehicleType { Type = "MC" },
                                                       new VehicleType { Type = "Airplane" },
                                                       new VehicleType { Type = "Spaceship" });


            context.Members.AddOrUpdate(v => v.Name, new Member { Name = "Tomas" });

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
