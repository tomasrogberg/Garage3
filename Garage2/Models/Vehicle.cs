using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Garage2.Models
{
    public enum VType { Car, MC, Boat, Plane, SpaceShip }

    public class Vehicle
    {

        public int Id { get; set; }

        public string Driver { get; set; }

        [ForeignKey("Member")]
        public int MemberId { get; set; }
        public virtual Member Member { get; set; }

        [Required]
        //public VType Type { get; set; }
        [ForeignKey("Type")]
        public int VTypeId { get; set; }
        public virtual VehicleType Type { get; set; }

        [StringLength(10, MinimumLength = 4)]
        [Required]
        public string RegNr { get; set; }

        public string Brand { get; set; }
        public string ProdName { get; set; }

        [Required]
        public string Color { get; set; }

        [Range(0, 9999)]
        public int Wheels { get; set; }
        public DateTime? CheckInTime { get; set; }
        //       public int ParkSpace { get; set; }

        public int ParkNr { get; set; }

    }
}