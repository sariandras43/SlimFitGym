using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym_Mobile.Models
{
    public class Machine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } = null;

        public static List<Machine> GetMachines()
        {
            var machines = new List<Machine>();
            machines.Add(new Machine { Id = 1, Name = "Treadmill", Description = "fdghfhdgfsfasdgas" });
            machines.Add(new Machine { Id = 2, Name = "Elliptical" });
            machines.Add(new Machine { Id = 3, Name = "Stationary Bike" });
            machines.Add(new Machine { Id = 4, Name = "Rowing Machine" });
            machines.Add(new Machine { Id = 5, Name = "Rowing Machine" });
            machines.Add(new Machine { Id = 6, Name = "Rowing Machine" });
            machines.Add(new Machine { Id = 7, Name = "Rowing Machine" });
            machines.Add(new Machine { Id = 8, Name = "Rowing Machine" });
            machines.Add(new Machine { Id = 9, Name = "Rowing Machine" });
            return machines;
        }
    }
}
