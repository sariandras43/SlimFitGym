using SlimFitGym.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Models.Requests
{
    public class RoomAndMachineRequest
    {
        public RoomAndMachineRequest()
        {
            
        }
        public RoomAndMachineRequest(RoomAndMachine rm)
        {
            Id = rm.Id;
            RoomId = rm.RoomId;
            MachineId = rm.MachineId;
            MachineCount = rm.MachineCount;
        }
        public int Id { get; set; }

        public int RoomId { get; set; }

        public int MachineId { get; set; }
        public int MachineCount { get; set; }
    }
}
