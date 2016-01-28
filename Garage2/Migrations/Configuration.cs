namespace Garage2.Migrations
{
    using Garage2.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Garage2.Models.Garage2Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Garage2.Models.Garage2Context";
        }

        protected override void Seed(Garage2.Models.Garage2Context context)
        {
            //  This method will be called after migrating to the latest version.

            if (context.Vehicles.Count()==0)
            {

              context.Vehicles.AddOrUpdate(
                v => v.RegNr,
                new Vehicle { RegNr = "AAA111", Type = VType.Car, Brand = "Audi", ProdName = "90", Color = "Red", Wheels = 4, CheckInTime=DateTime.Now, ParkNr=1 },
                new Vehicle { RegNr = "BBB222", Type = VType.Car, Brand = "Volvo", ProdName = "XC90", Color = "Black", Wheels = 4, CheckInTime = DateTime.Now, ParkNr=7 },
                new Vehicle { RegNr = "CCC333", Type = VType.Car, Brand = "Fiat", ProdName = "Uno", Color = "Yellow", Wheels = 4, CheckInTime = DateTime.Now, ParkNr=4 }
              
              );

            } else
            {

                Garage gar = new Garage(1000);
                var vehiclesParked = (from v in context.Vehicles
                                      where v.ParkNr > 0
                                      select v).ToList();

                var vehiclesUnParked = (from v in context.Vehicles
                                        where v.ParkNr <= 0
                                        select v).ToList();


                foreach (var veh in vehiclesParked)
                {
                    //  if (veh.ParkNr>0 && veh.ParkNr<=gar.Max)
                    gar.AddToParkNr(veh.ParkNr);  //om platsen redan finns !!??
                    //veh.ParkNr=0; //går ju inte

                }
                foreach (var veh in vehiclesUnParked)
                {
                    int p = gar.Add();
                    if (p > 0)
                    {
                        context.Vehicles.Where(v => v.Id == veh.Id).FirstOrDefault().ParkNr = (Int32)p;
                        context.SaveChanges();
                    }

                }

            }

            

           
        }

    

    }
}
