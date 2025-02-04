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

            //modelBuilder.Entity<Account>().HasData
            //(
            //    new Account() { Id = 1, Name = "Zöldség leves",Email="asd@asd.com",Role="admin",Phone="+36123456789" },
            //    new Account() { Id = 2, Name = "Sült krumpli", Email = "asd2@asd.com", Role = "user", Phone = "+36123456799" }
            //);

            modelBuilder.Entity<Machine>().HasData
            (
                new Machine() { Id = 1, Name = "Toló", Description = null },
                new Machine() { Id = 2, Name = "Húzó", Description = null }
            );
        }
    }
}
