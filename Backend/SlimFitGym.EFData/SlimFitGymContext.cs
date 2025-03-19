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
        public DbSet<RoomAndMachine> RoomsAndMachines { get; set; } = null!;
        public DbSet<Reservation> Reservations { get; set; } = null!;
        public DbSet<Purchase> Purchases { get; set; } = null!;
        public DbSet<TrainerApplicant> TrainerApplicants { get; set; } = null!;
        public DbSet<Entry> Entries { get; set; } = null!;
        public DbSet<Image> Images { get; set; } = null!;
        public string DbPath { get; }

        public SlimFitGymContext(IConfiguration configuration)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
#if DEBUG
            DbPath = System.IO.Path.Join(path, "test.db");

#else
            DbPath = System.IO.Path.Join(path, configuration["DatabaseName"] ?? "slimfitgym.db");
#endif

        }

        public SlimFitGymContext(DbContextOptions options,IConfiguration configuration) : base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
#if DEBUG
            DbPath = System.IO.Path.Join(path, "test.db");

#else
            DbPath = System.IO.Path.Join(path, configuration["DatabaseName"] ?? "slimfitgym.db");
#endif
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
            modelBuilder.Entity<Account>().HasData
            (
                new Account() { Id=1,Name="admin",Password= BCrypt.Net.BCrypt.EnhancedHashPassword("admin", 10), Email="admin@gmail.com",Phone="+36123456789",Role="admin"},
                new Account() { Id = 2, Name = "kazmer", Password = BCrypt.Net.BCrypt.EnhancedHashPassword("kazmer", 10), Email = "kazmer@gmail.com", Phone = "+36123456799", Role = "trainer" },
                new Account() { Id = 3, Name = "pista", Password = BCrypt.Net.BCrypt.EnhancedHashPassword("pista", 10), Email = "pista@gmail.com", Phone = "+36123456788", Role = "user" }
            );

            modelBuilder.Entity<Machine>().HasData
            (
                new Machine() { Id = 1, Name = "Bicikli", Description = null },
                new Machine() { Id = 2, Name = "Futópad", Description = null },
                new Machine() { Id = 3, Name = "Elliptikus tréner", Description = null },
                new Machine() { Id = 4, Name = "Evezőgép", Description = "Professzionális evezőgép" },
                new Machine() { Id = 5, Name = "Súlyzópad", Description = "Állítható dőlésszögű súlyzópad" },
                new Machine() { Id = 6, Name = "Lépcsőzőgép", Description = "Intenzív kardió edzéshez" },
                new Machine() { Id = 7, Name = "Mellprés gép", Description = "Mellizom erősítésére" },
                new Machine() { Id = 8, Name = "Hasprés gép", Description = "Hasizom erősítésére" },
                new Machine() { Id = 9, Name = "Guggoló állvány", Description = "Guggolás gyakorlására" },
                new Machine() { Id = 10, Name = "Hátlehúzó gép", Description = "Hátizom edzésére" }
            );

            modelBuilder.Entity<Room>().HasData
            (
                new Room() { Id = 1, Name = "Futós szoba", Description = "", RecommendedPeople = 15, IsActive = true },
                new Room() { Id = 2, Name = "Biciklizős Szoba", Description = "", RecommendedPeople = 10, IsActive = true },
                new Room() { Id = 3, Name = "Súlyzós terem", Description = "Súlyzós edzéshez felszerelve", RecommendedPeople = 20, IsActive = true },
                new Room() { Id = 4, Name = "CrossFit terem", Description = "Funkcionális edzésekhez", RecommendedPeople = 12, IsActive = true },
                new Room() { Id = 5, Name = "Jógaterem", Description = "Jóga és meditáció", RecommendedPeople = 8, IsActive = true },
                new Room() { Id = 6, Name = "Aerobik terem", Description = "Csoportos órákhoz", RecommendedPeople = 18, IsActive = true },
                new Room() { Id = 7, Name = "Küzdősport terem", Description = "Boksz és harcművészetek", RecommendedPeople = 10, IsActive = true },
                new Room() { Id = 8, Name = "Spinning terem", Description = "Csoportos biciklizés", RecommendedPeople = 15, IsActive = true },
                new Room() { Id = 9, Name = "Rehabilitációs terem", Description = "Gyógytorna és rehabilitáció", RecommendedPeople = 8, IsActive = true },
                new Room() { Id = 10, Name = "Kardió terem", Description = "Kardiógépekkel felszerelve", RecommendedPeople = 20, IsActive = true }
            );

            modelBuilder.Entity<RoomAndMachine>().HasData
            (
                new { Id = 1, MachineId = 1, RoomId = 2, MachineCount = 2 },
                new { Id = 2, MachineId = 2, RoomId = 1, MachineCount = 4 },
                new { Id = 3, MachineId = 3, RoomId = 1, MachineCount = 3 },
                new { Id = 4, MachineId = 4, RoomId = 5, MachineCount = 2 },
                new { Id = 5, MachineId = 5, RoomId = 3, MachineCount = 5 },
                new { Id = 6, MachineId = 6, RoomId = 4, MachineCount = 3 },
                new { Id = 7, MachineId = 7, RoomId = 3, MachineCount = 4 },
                new { Id = 8, MachineId = 8, RoomId = 6, MachineCount = 2 },
                new { Id = 9, MachineId = 9, RoomId = 7, MachineCount = 2 },
                new { Id = 10, MachineId = 10, RoomId = 10, MachineCount = 3 }
            );

            modelBuilder.Entity<Training>().HasData
            (
                new Training() { Id = 1, Name = "TRX edzés", RoomId = 1, TrainerId = 2, MaxPeople = 6, TrainingStart = DateTime.Now, TrainingEnd = DateTime.Now.AddHours(1) },
                new Training() { Id = 2, Name = "Spinning", RoomId = 8, TrainerId = 2, MaxPeople = 10, TrainingStart = DateTime.Now.AddHours(2), TrainingEnd = DateTime.Now.AddHours(3) },
                new Training() { Id = 3, Name = "Jóga", RoomId = 5, TrainerId = 2, MaxPeople = 8, TrainingStart = DateTime.Now.AddHours(1), TrainingEnd = DateTime.Now.AddHours(2) },
                new Training() { Id = 4, Name = "CrossFit", RoomId = 4, TrainerId = 2, MaxPeople = 12, TrainingStart = DateTime.Now.AddHours(3), TrainingEnd = DateTime.Now.AddHours(4) },
                new Training() { Id = 5, Name = "Aerobik", RoomId = 6, TrainerId = 2, MaxPeople = 18, TrainingStart = DateTime.Now.AddHours(5), TrainingEnd = DateTime.Now.AddHours(6) }
            );

            modelBuilder.Entity<Benefit>().HasData
            (
                new Benefit() { Id = 1, BenefitName = "Legjobb" },
                new Benefit() { Id = 2, BenefitName = "Kedvezményes ár" },
                new Benefit() { Id = 3, BenefitName = "VIP belépés" },
                new Benefit() { Id = 4, BenefitName = "Extra szolgáltatások" },
                new Benefit() { Id = 5, BenefitName = "Hosszabb edzésidő" }
            );

            modelBuilder.Entity<Pass>().HasData
            (
                new Pass() { Id = 1, Days = 30, Name = "Havi", MaxEntries = 0, Price = 15000 },
                new Pass() { Id = 2, Days = 0, Name = "15 alkalmas bérlet", MaxEntries = 15, Price = 10000 },
                new Pass() { Id = 3, Days = 90, Name = "Negyedéves", MaxEntries = 0, Price = 40000 },
                new Pass() { Id = 4, Days = 365, Name = "Éves bérlet", MaxEntries = 0, Price = 120000 },
                new Pass() { Id = 5, Days = 7, Name = "Heti bérlet", MaxEntries = 7, Price = 5000 }
            );

            modelBuilder.Entity<PassAndBenefit>().HasData
            (
                new PassAndBenefit() { Id = 1, BenefitId = 1, PassId = 1 },
                new PassAndBenefit() { Id = 2, BenefitId = 1, PassId = 2 },
                new PassAndBenefit() { Id = 3, BenefitId = 2, PassId = 3 },
                new PassAndBenefit() { Id = 4, BenefitId = 3, PassId = 4 },
                new PassAndBenefit() { Id = 5, BenefitId = 4, PassId = 5 }
            );

            modelBuilder.Entity<Reservation>().HasData
            (
                new Reservation() { Id=1,TrainingId=1,AccountId=3}
            );


            modelBuilder.Entity<Purchase>().HasData
            (
                new Purchase() {Id=1,AccountId=3,PassId=2,PurchaseDate= DateTime.Now }
            );

            modelBuilder.Entity<Benefit>().HasIndex(b => b.BenefitName).IsUnique();
            modelBuilder.Entity<Account>().HasIndex(a=>a.Email).IsUnique();
            modelBuilder.Entity<Machine>().HasIndex(m=>m.Name).IsUnique();
            modelBuilder.Entity<Room>().HasIndex(r=>r.Name).IsUnique();

        }

    }
}
