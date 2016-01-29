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
    public class Vehicles2Controller : Controller
    {
        private Garage2Context db = new Garage2Context();

        // GET: Vehicles2
        //public ActionResult Index()
        //{
        //    var vehicles = db.Vehicles.Include(v => v.Member).Include(v => v.Type);
        //    return View(vehicles.ToList());
        //}


        public ActionResult Index(string searchString, string sortOrder, string searchType)
        {

            //ViewBag.TypeSortParm = sortOrder == "type_desc" ? "type_asc" : "type_desc";
            ViewBag.RegNrSortParm = sortOrder == "regnr_desc" ? "regnr_asc" : "regnr_desc";
            ViewBag.BrandSortParm = sortOrder == "brand_desc" ? "brand_asc" : "brand_desc";
            ViewBag.CheckinTimeSortParm = sortOrder == "checkintime_desc" ? "checkintime_asc" : "checkintime_desc";
            ViewBag.ParkingTimeSortParm = sortOrder == "parkingtime_desc" ? "parkingtime_asc" : "parkingtime_desc";
            ViewBag.SlotNoSortParm = sortOrder == "slotno_desc" ? "slotno_asc" : "slotno_desc";
            ViewBag.SearchString = searchString;
            ViewBag.SearchType = searchType;

            //var vehicle = db.Vehicles.Include(v => v.Member).Include(v => v.Type);
            var vehicle = from v in db.Vehicles.Include(v => v.Member).Include(v => v.Type)
                          select v;

            if (!String.IsNullOrEmpty(searchString))
            {
                vehicle = vehicle.Where(s => s.RegNr.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(searchType))
            {
                vehicle = vehicle.Where(s => s.Type.ToString().Contains(searchType));
            }

            switch (sortOrder)
            {
                case "regnr_desc":
                    vehicle = vehicle.OrderByDescending(v => v.RegNr);
                    break;
                case "regnr_asc":
                    vehicle = vehicle.OrderBy(v => v.RegNr);
                    break;
                //case "type_desc":
                //    vehicle = vehicle.OrderByDescending(v => v.Type);
                //    break;
                //case "type_asc":
                //    vehicle = vehicle.OrderBy(v => v.Type);
                //    break;
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
                    vehicle = vehicle.OrderBy(v => v.RegNr);
                    break;
            }
            List<Vehicle> vehicleList = vehicle.ToList();
            return View(vehicleList);


        }


        // GET: Vehicles2/Details/5
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

        // GET: Vehicles2/Create
        public ActionResult Create()
        {
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Name");
            ViewBag.VTypeId = new SelectList(db.VehicleTypes, "Id", "Type");
            return View();
        }

        // POST: Vehicles2/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //        public ActionResult Create([Bind(Include = "Id,MemberId,VTypeId,RegNr,Brand,ProdName,Color,Wheels,CheckInTime,ParkNr")] Vehicle vehicle)
        public ActionResult Create([Bind(Include = "Id,Driver,VTypeId,RegNr,Brand,ProdName,Color,Wheels")] Vehicle vehicle)
        {
            string Driver = vehicle.Driver;
            var me = from v in db.Members
                     where v.Name == Driver
                     select v;
           if (me.Count() != 0)
            {
                Member mem = me.First();
                if (mem != null)
                {
                    vehicle.MemberId = mem.Id;
                    if (ModelState.IsValid)
                    {
                        vehicle.CheckInTime = DateTime.Now;
                        db.Vehicles.Add(vehicle);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
           } else
            {
                return RedirectToAction("Index", "Members");
            }
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Name", vehicle.MemberId);
            ViewBag.VTypeId = new SelectList(db.VehicleTypes, "Id", "Type", vehicle.VTypeId);
            return View(vehicle);
        }

        // GET: Vehicles2/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Name", vehicle.MemberId);
            ViewBag.VTypeId = new SelectList(db.VehicleTypes, "Id", "Type", vehicle.VTypeId);
            return View(vehicle);
        }

        // POST: Vehicles2/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MemberId,VTypeId,RegNr,Brand,ProdName,Color,Wheels,CheckInTime,ParkNr")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehicle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Name", vehicle.MemberId);
            ViewBag.VTypeId = new SelectList(db.VehicleTypes, "Id", "Type", vehicle.VTypeId);
            return View(vehicle);
        }

        // GET: Vehicles2/Delete/5
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

        // POST: Vehicles2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            db.Vehicles.Remove(vehicle);
            db.SaveChanges();
            return RedirectToAction("Index");
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
