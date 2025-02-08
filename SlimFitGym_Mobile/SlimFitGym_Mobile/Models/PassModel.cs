using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym_Mobile.Models
{
    public class PassModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxEntries { get; set; }
        public int Days { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }

        public static List<PassModel> GetPasses()
        {
            List<PassModel> passes = new List<PassModel>();
            passes.Add(new PassModel { Id = 1, Name = "Daily", Description = "Daily pass", MaxEntries = 1, Days = 1, Price = 10, IsActive = true });
            passes.Add(new PassModel { Id = 2, Name = "Weekly", Description = "Weekly pass", MaxEntries = 7, Days = 7, Price = 50, IsActive = true });
            passes.Add(new PassModel { Id = 3, Name = "Monthly", Description = "Monthly pass", MaxEntries = 30, Days = 30, Price = 150, IsActive = true });
            passes.Add(new PassModel { Id = 4, Name = "Yearly", Description = "Yearly pass", MaxEntries = 365, Days = 365, Price = 500, IsActive = true });
            return passes;
        }
    }
}
