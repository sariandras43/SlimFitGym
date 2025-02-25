using SlimFitGym.Models.Models;
using SlimFitGym.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Models.Responses
{
    public class RoomAndMachineResponse
    {
        public RoomAndMachineResponse()
        {

        }
        public RoomAndMachineResponse(RoomAndMachine rm)
        {
            Id = rm.Id;
            RoomId = rm.RoomId;
            MachineId = rm.MachineId;
            MachineCount = rm.MachineCount;
        }

        public RoomAndMachineResponse(RoomAndMachineRequest rm)
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
