using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using SlimFitGym.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.EFData
{
    public class SlimFitGymContext : DbContext
    {
        //public readonly IConfiguration config;
        public DbSet<RoomAndMachine> RoomsAndMachines { get; set; } = null!;
        public DbSet<Reservation> Reservations { get; set; } = null!;
        public DbSet<Purchase> Purchases { get; set; } = null!;
        public DbSet<TrainerApplicant> TrainerApplicants { get; set; } = null!;
        public DbSet<Entry> Entries { get; set; } = null!;
        public string DbPath { get; }

        public SlimFitGymContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            //DbPath = System.IO.Path.Join(path, config["DatabaseName"]??"slimfitgym.db");
            DbPath = System.IO.Path.Join(path, "slimfitgym.db");

        }

        public SlimFitGymContext(DbContextOptions options,IConfiguration configuration) : base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "slimfitgym.db");
            //DbPath = System.IO.Path.Join(path, config["DatabaseName"] ?? "slimfitgym.db");
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
                new Room() { Id = 1, Name = "Futós szoba", Description = "", RecommendedPeople = 15, IsActive=true },
                new Room() { Id = 2, Name = "Biciklizős Szoba", Description = "", RecommendedPeople = 10,IsActive=true }
            );
            modelBuilder.Entity<RoomAndMachine>().HasData
            (
                new { Id = 1, MachineId = 1, RoomId = 2, MachineCount=1 },
                new { Id = 2, MachineId = 2, RoomId = 1, MachineCount = 4 }
            );
            
            modelBuilder.Entity<Account>().HasData
            (
                new Account() { Id=1,Name="admin",Password= BCrypt.Net.BCrypt.EnhancedHashPassword("admin", 10), Email="admin@gmail.com",Phone="+36123456789",Role="admin"},
                new Account() { Id = 2, Name = "kazmer", Password = BCrypt.Net.BCrypt.EnhancedHashPassword("kazmer", 10), Email = "kazmer@gmail.com", Phone = "+36123456799", Role = "trainer" },
                new Account() { Id = 3, Name = "pista", Password = BCrypt.Net.BCrypt.EnhancedHashPassword("pista", 10), Email = "pista@gmail.com", Phone = "+36123456788", Role = "user" }
            );

            modelBuilder.Entity<Training>().HasData
            (
                new Training() { Id=1, Name="TRX edzés",RoomId=1,TrainerId=2,MaxPeople=1,Description="Izmos személyek számára",TrainingStart=new DateTime(2025,2,6,17,0,0),TrainingEnd= new DateTime(2025, 2, 6, 18, 0, 0) }
            );

            modelBuilder.Entity<Reservation>().HasData
            (
                new Reservation() { Id=1,TrainingId=1,AccountId=3}
            );


            modelBuilder.Entity<Benefit>().HasData
            (
                new Benefit() { Id = 1, BenefitName="Legjobb" }
            );

            modelBuilder.Entity<Pass>().HasData
            (
                new Pass() { Id=1,Days=30,Name="Havi",MaxEntries=0,Price=15000}
            );
            modelBuilder.Entity<PassAndBenefit>().HasData
            (
                new PassAndBenefit() { Id=1,BenefitId=1,PassId=1}
            );
            modelBuilder.Entity<Purchase>().HasData
            (
                new Purchase() {Id=1,AccountId=1,PassId=1,PurchaseDate= new DateTime(2025, 2, 6, 17, 0, 0) }
            );

            modelBuilder.Entity<Benefit>().HasIndex(b => b.BenefitName).IsUnique();
            modelBuilder.Entity<Account>().HasIndex(a=>a.Email).IsUnique();
            modelBuilder.Entity<Machine>().HasIndex(m=>m.Name).IsUnique();
            modelBuilder.Entity<Room>().HasIndex(r=>r.Name).IsUnique();

            modelBuilder.Entity<Entry>().HasData();
        }


        //Supposed to work, but doesn't
        //public override int SaveChanges()
        //{
        //    IEnumerable<ValidationResult> validationErrors = ChangeTracker
        //        .Entries<IValidatableObject>()
        //        .SelectMany(e => e.Entity.Validate(null!))
        //        .Where(r => r != ValidationResult.Success);

        //    if (validationErrors.Any())
        //    {
        //        try
        //        {

        //        }

        //        catch (DbUpdateException exception)
        //        {
        //            throw new Exception("Nem megfelelő formátumú");
        //        }
        //    }

        //    return base.SaveChanges();
        //}
    }
}
