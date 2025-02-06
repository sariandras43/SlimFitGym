using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SlimFitGym.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.EFData
{
    public class SlimFitGymContext : DbContext
    {
        public DbSet<RoomAndMachine> RoomsAndMachines { get; set; } = null!;
        public DbSet<Reservation> Reservations { get; set; } = null!;
        public string DbPath { get; }

        public SlimFitGymContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            //TODO: write it into a config file
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
            //add-migration XYZ -Project SlimFitGym.EFData -StartupProject SlimFitGym.EFData

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
                new { Id = 1, MachineId = 1, RoomId = 2, MachineCount=1 },
                new { Id = 2, MachineId = 2, RoomId = 1, MachineCount = 4 }
            );

            modelBuilder.Entity<Account>().HasData
            (
                new Account() { Id=1,Name="admin",Password="admin",Email="admin@gmail.com",Phone="+36123456789",Role="admin"},
                new Account() { Id = 2, Name = "kazmer", Password = "kazmer", Email = "kazmer@gmail.com", Phone = "+36123456799", Role = "trainer" },
                new Account() { Id = 3, Name = "pista", Password = "pista", Email = "pista@gmail.com", Phone = "+36123456788", Role = "user" }
            );

            modelBuilder.Entity<Training>().HasData
            (
                new Training() { Id=1, Name="TRX edzés",RoomId=1,TrainerId=2,MaxPeople=1,TrainingStart=new DateTime(2025,2,6,17,0,0),TrainingEnd= new DateTime(2025, 2, 6, 18, 0, 0) }
            );

            modelBuilder.Entity<Reservation>().HasData
            (
                new Reservation() { Id=1,TrainingId=1,AccountId=3}
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
