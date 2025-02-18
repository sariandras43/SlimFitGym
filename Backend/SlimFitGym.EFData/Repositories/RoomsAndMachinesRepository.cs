using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SlimFitGym.Models.Models;
using SlimFitGym.Models.Requests;
using SlimFitGym.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static SlimFitGym.EFData.Repositories.MachinesRepository;
using Machine = SlimFitGym.Models.Models.Machine;

namespace SlimFitGym.EFData.Repositories
{
    public class RoomsAndMachinesRepository
    {
        readonly SlimFitGymContext context;

        public RoomsAndMachinesRepository(SlimFitGymContext context)
        {
            this.context = context;
        }

        public List<RoomWithMachinesResponse>? GetRoomsWithMachines()
        {
            var result = context.Set<Room>()
                .Select(room => new
                {
                    room,
                    RoomAndMachines = context.RoomsAndMachines
                        .Where(rm => rm.RoomId == room.Id) 
                        .ToList()  
                })
                .AsEnumerable() 
                .Select(x => new RoomWithMachinesResponse
                {
                    Id = x.room.Id,
                    Name = x.room.Name,
                    Description = x.room.Description!,
                    RecommendedPeople = x.room.RecommendedPeople,
                    Machines = x.RoomAndMachines == null || !x.RoomAndMachines.Any()
                        ? new List<MachineDetails>()  
                        : x.RoomAndMachines
                            .Select(rm => new MachineDetails
                            {
                                Id = rm.MachineId,
                                Name = context.Set<Machine>()
                                    .Where(m => m.Id == rm.MachineId)
                                    .Select(m => m.Name)
                                    .FirstOrDefault()!, 
                                MachineCount = rm.MachineCount
                            })
                            .ToList()
                })
                .ToList();

            return result;
        }

        public RoomWithMachinesResponse? GetRoomWithMachinesById(int id)
        {
            var result = context.Set<Room>()
                .Select(room => new
                {
                    room,
                    RoomAndMachines = context.RoomsAndMachines
                        .Where(rm => rm.RoomId == room.Id)
                        .ToList()
                })
                .AsEnumerable()
                .Select(x => new RoomWithMachinesResponse
                {
                    Id = x.room.Id,
                    Name = x.room.Name,
                    Description = x.room.Description!,
                    RecommendedPeople = x.room.RecommendedPeople,
                    Machines = x.RoomAndMachines == null || !x.RoomAndMachines.Any()
                        ? new List<MachineDetails>()
                        : x.RoomAndMachines
                            .Select(rm => new MachineDetails
                            {
                                Id = rm.MachineId,
                                Name = context.Set<Machine>()
                                    .Where(m => m.Id == rm.MachineId)
                                    .Select(m => m.Name)
                                    .FirstOrDefault()!,
                                MachineCount = rm.MachineCount
                            })
                            .ToList()
                }).SingleOrDefault(r=>r.Id==id);

            return result;
        }

        public RoomAndMachineResponse? GetRoomAndMachineConnectionById(int id)
        {
            RoomAndMachine? roomAndMachine = context.Set<RoomAndMachine>().SingleOrDefault(rm => rm.Id == id);
            if (roomAndMachine != null)
                return new RoomAndMachineResponse(roomAndMachine);
            return null;

        }

        public List<RoomAndMachine>? GetRoomsAndMachinesByRoomId(int roomId)
        {
            List<RoomAndMachine>? roomsAndMachines = context.Set<RoomAndMachine>().Where(rm=>rm.RoomId==roomId).ToList();
            if (roomsAndMachines != null)
                return roomsAndMachines;
            return null;
        }

        public RoomAndMachine? GetRoomAndMachineByMachineAndRoomId(int machineId, int roomId)
        {
            RoomAndMachine? roomAndMachine = context.Set<RoomAndMachine>().SingleOrDefault(rm => rm.MachineId == machineId && rm.RoomId == roomId);
            if (roomAndMachine != null)
                return roomAndMachine;
            return null;
        }

        public List<RoomAndMachine>? GetRoomAndMachineConnections()
        {
            //return context.Set<RoomAndMachine>().Select(rm=>new RoomAndMachineResponse(rm)).ToList();
            return context.Set<RoomAndMachine>().ToList();
        }

        public RoomAndMachineResponse? ConnectRoomAndMachine(RoomAndMachine roomAndMachine)
        {
            if (!context.Set<Machine>().Any(m=>m.Id == roomAndMachine.MachineId))
                throw new Exception("Ez a gép nem létezik.");
            if (roomAndMachine.MachineCount < 1)
                throw new Exception("Érvénytelen gépszám.");
            if (!context.Set<Room>().Any(r => r.Id == roomAndMachine.RoomId))
                throw new Exception("Ez a terem nem létezik.");
            if (context.Set<RoomAndMachine>().Any(rm=>rm.MachineId==roomAndMachine.MachineId &&rm.RoomId==roomAndMachine.RoomId))
                throw new Exception("Ehhez a teremhez már hozzá van rendelve ez a gép.");

            RoomAndMachine savedRoomAndMachine = this.context.Set<RoomAndMachine>().Add(roomAndMachine).Entity;

            this.context.SaveChanges();
            return new RoomAndMachineResponse(savedRoomAndMachine);
        }

        public RoomAndMachineResponse? UpdateRoomAndMachineConnection(int id, RoomAndMachineRequest rm)
        {
            if (id != rm.Id)
                throw new Exception("Érvénytelen azonosító.");
            if (rm.MachineCount < 1)
                throw new Exception("Érvénytelen gépszám.");
            if (!context.Set<RoomAndMachine>().Any(r => r.Id == rm.Id))
                return null;
            //if (context.Set<RoomAndMachine>().Any(x => x.MachineId == rm.MachineId && x.RoomId == rm.RoomId))
            //    throw new Exception("Ehhez a teremhez már hozzá van rendelve ez a gép.");
            if (!context.Set<Machine>().Any(m => m.Id == rm.MachineId))
                throw new Exception("Ez a gép nem létezik");
            if (!context.Set<Room>().Any(r => r.Id == rm.RoomId))
                throw new Exception("Ez a terem nem létezik");

            RoomAndMachine modifiedRoomAndMachineConnection = new RoomAndMachine
            {
                Id=id,
                MachineCount=rm.MachineCount,
                MachineId=rm.MachineId,
                RoomId= rm.RoomId
            };

            this.context.Entry(modifiedRoomAndMachineConnection).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            this.context.SaveChanges();
            return new RoomAndMachineResponse(modifiedRoomAndMachineConnection);
        }

        public RoomAndMachineResponse? DeleteConnection(int id)
        {
            RoomAndMachine roomAndMachineToDelete = context.Set<RoomAndMachine>().SingleOrDefault(rm=>rm.Id==id);
            if (roomAndMachineToDelete == null)
                return null;

            this.context.Set<RoomAndMachine>().Remove(roomAndMachineToDelete);
            this.context.SaveChanges();
            return new RoomAndMachineResponse(roomAndMachineToDelete);
        }




    }
}
