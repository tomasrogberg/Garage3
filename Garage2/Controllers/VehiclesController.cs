using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Garage2.Models;


namespace Garage2.Controllers
{
    public class VehiclesController : Controller
    {
        public Garage2Context db = new Garage2Context();

        int AddToFirstEmptySpace(VType vType) // returnera första lediga plats med eventuella nödvändiga platser extra efter varandra
        {
            Garage gar = new Garage(1000);
            var vehiclesParked = (from v in db.Vehicles
                                  select v).ToList();



            foreach (var veh in vehiclesParked)
            {
                gar.AddToParkNr(veh.ParkNr);  //om platsen redan finns !!??
                //veh.ParkNr=0; //går ju inte

            }
            
            return gar.Add();
        }


 

        // GET: Vehicles

        public ActionResult Index(string searchString, string sortOrder )
        {

            ViewBag.TypeSortParm = sortOrder == "type_desc" ? "type_asc" : "type_desc";
            ViewBag.RegNrSortParm = sortOrder == "regnr_desc" ? "regnr_asc" : "regnr_desc";
            ViewBag.BrandSortParm = sortOrder == "brand_desc" ? "brand_asc" : "brand_desc";
            ViewBag.CheckinTimeSortParm = sortOrder == "checkintime_desc" ? "checkintime_asc" : "checkintime_desc";
            ViewBag.ParkingTimeSortParm = sortOrder == "parkingtime_desc" ? "parkingtime_asc" : "parkingtime_desc";
            ViewBag.SlotNoSortParm = sortOrder == "slotno_desc" ? "slotno_asc" : "slotno_desc";
            ViewBag.SearchString = searchString;

            var vehicle = from v in db.Vehicles
                          select v;

            if (!String.IsNullOrEmpty(searchString))
            {
                vehicle = vehicle.Where(s => s.RegNr.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "regnr_desc":
                    vehicle = vehicle.OrderByDescending(v => v.RegNr);
                    break;
                case "regnr_asc":
                    vehicle = vehicle.OrderBy(v => v.RegNr);
                    break;
                case "type_desc":
                    vehicle = vehicle.OrderByDescending(v => v.Type);
                    break;
                case "type_asc":
                    vehicle = vehicle.OrderBy(v => v.Type);
                    break;
                case "brand_desc":
                    vehicle = vehicle.OrderByDescending(v => v.Brand);
                    break;
                case "brand_asc":
                    vehicle = vehicle.OrderBy(v => v.Brand);
                    break;
                case "checkintime_desc":
                    vehicle = vehicle.OrderByDescending(v => v.CheckInTime);
                    break;
                case "checkintime_asc":
                    vehicle = vehicle.OrderBy(v => v.CheckInTime);
                    break;
                case "parkingtime_desc":
                    vehicle = vehicle.OrderByDescending(v => v.CheckInTime);
                    break;
                case "parkingtime_asc":
                    vehicle = vehicle.OrderBy(v => v.CheckInTime);
                    break;
                case "slotno_desc":
                    vehicle = vehicle.OrderByDescending(v => v.ParkNr);
                    break;
                case "slotno_asc":
                    vehicle = vehicle.OrderBy(v => v.ParkNr);
                    break;

                default:
                    vehicle = vehicle.OrderBy(v => v.Type);
                    break;
            }

            return View(vehicle);

       
        }

        // GET: Vehicles/Details/5

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // GET: Vehicles/Create
        public ActionResult Create()
        {
            if (TempData["SetConfirmPark"] != null)
            {
                ViewBag.ConfirmPark = TempData["SetConfirmPark"];
            }
            else
            {
                ViewBag.ConfirmPark = "Please enter your vehicle details.";
            }

            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Type,RegNr,Brand,ProdName,Color,Wheels,CheckInTime")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                vehicle.CheckInTime = DateTime.Now;
                vehicle.ParkNr = AddToFirstEmptySpace(vehicle.Type); // fixa parkeringsnummer
                db.Vehicles.Add(vehicle);
                db.SaveChanges();

                TempData["SetConfirmPark"] = "Your " + vehicle.Type + " with regnr: " + vehicle.RegNr + " is now parked in the garage.";

                return RedirectToAction("Create");     // Ändrades till "Create" för att skapa loopen.
            }
            return View(vehicle);
        }

 
        // GET: Vehicles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);

        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            
            Vehicle vehicle = db.Vehicles.Find(id);
           
           // DeleteFromGarage(vehicle);

            db.Vehicles.Remove(vehicle);
            db.SaveChanges();

            return RedirectToAction("Receipt", vehicle);
            
        }

        public ActionResult Receipt(Vehicle vehicle)
        { 

            DateTime CheckOutTime = DateTime.Now;
            TimeSpan? ParkingTime = new TimeSpan();

            ParkingTime = CheckOutTime - vehicle.CheckInTime;

            double totH = ParkingTime.Value.TotalHours;
            int totHours = Convert.ToInt32(Math.Truncate(totH));

            var result = string.Format("{0:D2}:{1:D2}", totHours, ParkingTime.Value.Minutes);

            double pay = ParkingTime.Value.TotalHours * 60;
            string Payment = String.Format("{0:0}", pay);

            ViewBag.CheckOutTime = CheckOutTime;
            ViewBag.Payment = Payment;
            ViewBag.ParkingTime = result;

            return View(vehicle);

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
