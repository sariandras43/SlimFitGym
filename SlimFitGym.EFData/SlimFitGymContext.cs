using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SlimFitGym.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.EFData
{
    public class SlimFitGymContext : DbContext
    {
        //public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Machine> Machines { get; set; } = null!;
        public DbSet<RoomAndMachine> RoomsAndMachines { get; set; } = null!;
        public DbSet<Room> Rooms { get; set; } = null!;
        public string DbPath { get; }

        public SlimFitGymContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "slimfitgym.db");
        }

        public SlimFitGymContext(DbContextOptions options) : base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "slimfitgym.db");
        }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {

                options.UseSqlite($"Data Source={DbPath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Machine>().HasData
            (
                new Machine() { Id = 1, Name = "Bicikli", Description = null },
                new Machine() { Id = 2, Name = "Futópad", Description = null }
            );

            modelBuilder.Entity<Room>().HasData
            (
                new Room() { Id = 1, Name = "Futós szoba", Description = "", RecommendedPeople = 15 },
                new Room() { Id = 2, Name = "Biciklizős Szoba", Description = "", RecommendedPeople = 10 }
            );

            modelBuilder.Entity<RoomAndMachine>().HasData
            (
                new RoomAndMachine() { Id = 1, MachineId = 1, RoomId = 2 },
                new RoomAndMachine() { Id = 2, MachineId = 2, RoomId = 1 }
            );


        }

        //public override int SaveChanges()
        //{
        //    try
        //    {
        //        return base.SaveChanges();

        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        throw new Exception("Típushiba");
        //    }
        //}
    }
}
