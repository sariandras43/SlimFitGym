using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym_Mobile.Models
{
    public class MachineModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } = null;

        public static List<MachineModel> GetMachines()
        {
            var machines = new List<MachineModel>();
            machines.Add(new MachineModel { Id = 1, Name = "Treadmill", Description = "fdghfhdgfsfasdgas" });
            machines.Add(new MachineModel { Id = 2, Name = "Elliptical" });
            machines.Add(new MachineModel { Id = 3, Name = "Stationary Bike" });
            machines.Add(new MachineModel { Id = 4, Name = "Rowing Machine" });
            machines.Add(new MachineModel { Id = 5, Name = "Rowing Machine" });
            machines.Add(new MachineModel { Id = 6, Name = "Rowing Machine" });
            machines.Add(new MachineModel { Id = 7, Name = "Rowing Machine" });
            machines.Add(new MachineModel { Id = 8, Name = "Rowing Machine" });
            machines.Add(new MachineModel { Id = 9, Name = "Rowing Machine" });
            return machines;
        }
    }
}
