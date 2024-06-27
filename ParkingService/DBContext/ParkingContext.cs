using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace ParkingService.DBContext
{

    public class ParkingContext : DbContext
    {
        public ParkingContext(DbContextOptions<ParkingContext> options)
        : base(options)
        {
        }

        public DbSet<VehicleDetails> VehicleDetails { get; set; }
    }


    public class VehicleDetails
    {
        [Key]
        public int RFID { get; set; }
        public string? VehicleNum { get; set; }

        public string? GID { get; set; }

        public long? MobileNum { get; set; }

        public int Slot {  get; set; }


    }
}
