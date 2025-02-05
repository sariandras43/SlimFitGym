using Microsoft.EntityFrameworkCore;
using SlimFitGym.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static SlimFitGym.EFData.Repositories.MachinesRepository;
using Machine = SlimFitGym.Models.Machine;

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

        public RoomAndMachine? GetRoomAndMachineConnectionById(int id)
        {
            RoomAndMachine? roomAndMachine = context.Set<RoomAndMachine>().SingleOrDefault(rm => rm.Id == id);
            if (roomAndMachine == null)
                throw new Exception("Nincs ilyen gép-terem összeköttetés");
            return roomAndMachine;

        }

        public List<RoomAndMachine>? GetRoomAndMachineConnections()
        {
            return context.Set<RoomAndMachine>().ToList();
        }

        public RoomAndMachine? ConnectRoomAndMachine(RoomAndMachine roomAndMachine)
        {
            if (!context.Set<Machine>().Any(m=>m.Id == roomAndMachine.MachineId))
                throw new Exception("Ez a gép nem létezik.");
            if (!context.Set<Room>().Any(r => r.Id == roomAndMachine.RoomId))
                throw new Exception("Ez a terem nem létezik.");
            if (context.Set<RoomAndMachine>().Any(rm=>rm.MachineId==roomAndMachine.MachineId &&rm.RoomId==roomAndMachine.RoomId))
                throw new Exception("Ehhez a teremhez már hozzá van rendelve ez a gép.");

            RoomAndMachine savedRoomAndMachine = this.context.Set<RoomAndMachine>().Add(roomAndMachine).Entity;

            this.context.SaveChanges();
            return savedRoomAndMachine;
        }

        public RoomAndMachine? UpdateRoomAndMachineConnection(int id, RoomAndMachine rm)
        {
            if (id != rm.Id)
                throw new Exception("Érvénytelen azonosító.");
            if (!context.Set<RoomAndMachine>().Any(r => r.Id == rm.Id))
                throw new Exception("Ez a gép-terem kapcsolat nem létezik");
            if (context.Set<RoomAndMachine>().Any(x => x.MachineId == rm.MachineId && x.RoomId == rm.RoomId))
                throw new Exception("Ehhez a teremhez már hozzá van rendelve ez a gép.");
            if (!context.Set<Machine>().Any(m => m.Id == rm.Id))
                throw new Exception("Ez a gép nem létezik");
            if (!context.Set<Room>().Any(r => r.Id == rm.Id))
                throw new Exception("Ez a terem nem létezik");

            this.context.Entry(rm).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            this.context.SaveChanges();
            return rm;
        }

        public RoomAndMachine? DeleteConnection(int id)
        {
            RoomAndMachine roomAndMachineToDelete = context.Set<RoomAndMachine>().SingleOrDefault(rm=>rm.Id==id);
            if (roomAndMachineToDelete == null)
                throw new Exception("Nem létezik ez a gép-terem kapcsolat");

            this.context.Set<RoomAndMachine>().Remove(roomAndMachineToDelete);
            this.context.SaveChanges();
            return roomAndMachineToDelete;
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
