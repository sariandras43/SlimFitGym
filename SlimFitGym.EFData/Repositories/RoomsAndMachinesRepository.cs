using SlimFitGym.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SlimFitGym.EFData.Repositories.MachinesRepository;

namespace SlimFitGym.EFData.Repositories
{
    public class RoomsAndMachinesRepository
    {
        readonly SlimFitGymContext context;

        public RoomsAndMachinesRepository(SlimFitGymContext context)
        {
            this.context = context;
        }

        public List<RoomWithMachines>? GetRoomsWithMachines()
        {
            var result = context.RoomsAndMachines
                .Join(context.Rooms,
                      roomAndMachine => roomAndMachine.RoomId, 
                      room => room.Id,                         
                      (roomAndMachine, room) => new { roomAndMachine, room }) 
                .Join(context.Machines,
                      roomAndMachine => roomAndMachine.roomAndMachine.MachineId,
                      machine => machine.Id,
                      (roomAndMachineAndRoom, machine) => new
                      {
                          roomAndMachineAndRoom.roomAndMachine,
                          roomAndMachineAndRoom.room,
                          machine
                      })
                .GroupBy(x => x.room) 
                .Select(group => new RoomWithMachines
                {
                    Id = group.Key.Id, 
                    Name = group.Key.Name, 
                    Machines = group.Select(x => new MachineDetails
                    {
                        Id = x.machine.Id,  
                        Name = x.machine.Name 
                    }).ToList()  
                })
                .ToList();

            return result;
        }

        public RoomWithMachines GetRoomWithMachinesById(int id)
        {
            var result = context.RoomsAndMachines
                .Join(context.Rooms,
                      roomAndMachine => roomAndMachine.RoomId, 
                      room => room.Id,                         
                      (roomAndMachine, room) => new { roomAndMachine, room })  
                .Join(context.Machines,
                      roomAndMachine => roomAndMachine.roomAndMachine.MachineId, 
                      machine => machine.Id,  
                      (roomAndMachineAndRoom, machine) => new
                      {
                          roomAndMachineAndRoom.roomAndMachine,
                          roomAndMachineAndRoom.room,
                          machine
                      })
                .Where(r=>r.roomAndMachine.RoomId == id)
                .GroupBy(x => x.room)  
                .Select(group => new RoomWithMachines
                {
                    Id = group.Key.Id, 
                    Name = group.Key.Name,
                    Machines = group.Select(x => new MachineDetails
                    {
                        Id = x.machine.Id,  
                        Name = x.machine.Name  
                    }).ToList()  
                })
                .FirstOrDefault();

            return result;
        }

        public class RoomWithMachines
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public List<MachineDetails> Machines { get; set; }
        }

        public class MachineDetails
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
