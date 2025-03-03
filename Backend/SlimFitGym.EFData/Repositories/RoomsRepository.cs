using Microsoft.EntityFrameworkCore;
using SlimFitGym.Models.Models;
using SlimFitGym.Models.Requests;
using SlimFitGym.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Machine = SlimFitGym.Models.Models.Machine;

namespace SlimFitGym.EFData.Repositories
{
    public class RoomsRepository
    {
        readonly SlimFitGymContext context;
        readonly RoomsAndMachinesRepository roomsAndMachinesRepository;
        readonly ImagesRepository imagesRepository;

        public RoomsRepository(SlimFitGymContext context, RoomsAndMachinesRepository roomsAndMachinesRepository, ImagesRepository imagesRepository)
        {
            this.context = context;
            this.roomsAndMachinesRepository = roomsAndMachinesRepository;
            this.imagesRepository = imagesRepository;
        }

        public List<Room> GetAllRooms() 
        {
            return context.Set<Room>().ToList();
        }

        public Room? GetRoomById(int id)
        {
            Room? result = context.Set<Room>().ToList().SingleOrDefault(r => r.Id == id && r.IsActive);
            if (result == null)
                return null;
            
            return result;
        }

        public RoomWithMachinesResponse? NewRoom(RoomRequest newRoom)
        {

            if (newRoom == null)
                throw new Exception("Hibás kérés.");
            if (newRoom.Name == null || newRoom.Name.Length == 0)
                throw new Exception("A név mező kitöltése kötelező.");
            if (newRoom.Description == null || newRoom.Description.Length == 0)
                throw new Exception("A leírás mező kitöltése kötelező.");
            if (newRoom.RecommendedPeople <= 0)
                throw new Exception("A javasolt befogadóképesség mező kitöltése kötelező");
            if (newRoom.Name.Length > 100)
                throw new Exception("A név maximum 100 karakter hosszú lehet.");
            if (newRoom.Description != null && newRoom.Description!.Length > 500)
                throw new Exception("A leírás maximum 500 karakter hosszú lehet.");
            if (context.Set<Room>().Any(r => r.Name == newRoom.Name && r.IsActive))
                throw new Exception("Ilyen terem már létezik.");

            Room savedRoom;
            if (newRoom.Machines?.Count>0)
            {
                foreach (MachineForRoom mr in newRoom.Machines)
                {
                    if (!context.Set<Machine>().Any(m => m.Id == mr.Id))
                        throw new Exception($"Az {mr.Id} azonosítójú gép nem létezik.");
                }
                savedRoom = this.context.Set<Room>().Add(new Room() { Name=newRoom.Name,Description=newRoom.Description,RecommendedPeople=newRoom.RecommendedPeople, IsActive=true}).Entity;
                this.context.SaveChanges();

                foreach (MachineForRoom mr in newRoom.Machines)
                {
                    RoomAndMachine roomAndMachine = new RoomAndMachine() { MachineId=mr.Id,RoomId=savedRoom.Id,MachineCount=mr.Count};
                    roomsAndMachinesRepository.ConnectRoomAndMachine(roomAndMachine);
                    this.context.SaveChanges();

                }
            }
            else
            {
                savedRoom = this.context.Set<Room>().Add(new Room() { Name = newRoom.Name, Description = newRoom.Description, RecommendedPeople = newRoom.RecommendedPeople }).Entity;
                this.context.SaveChanges();

            }

            List<Image> images = imagesRepository.UploadImagesToRoom(newRoom.Images, savedRoom.Id);

            return roomsAndMachinesRepository.GetRoomWithMachinesById(savedRoom.Id);

        }

        public RoomWithMachinesResponse? UpdateRoom(int id,RoomRequest room)
        {

            if (id != room.Id)
                throw new Exception("Érvénytelen azonosító.");
            if (!this.context.Set<Room>().Any(r => r.Id == id))
                return null;
            if (room == null)
                throw new Exception("Hibás kérés.");
            if (room.Name == null || room.Name.Length == 0)
                throw new Exception("A név mező kitöltése kötelező.");
            if (room.Description == null || room.Description.Length == 0)
                throw new Exception("A leírás mező kitöltése kötelező.");
            if (room.RecommendedPeople == 0)
                throw new Exception("A javasolt befogadóképesség mező kitöltése kötelező");
            if (room.Name.Length > 100)
                throw new Exception("A név maximum 100 karakter hosszú lehet.");
            if (room.Description != null && room.Description!.Length > 500)
                throw new Exception("A leírás maximum 500 karakter hosszú lehet.");
            if (context.Set<Room>().Any(r => r.Name == room.Name && r.IsActive))
                throw new Exception("Ilyen terem már létezik.");
            Room modifiedRoom = new Room
            {
                Id = room.Id,
                Name = room.Name,
                Description = room.Description,
                RecommendedPeople = room.RecommendedPeople,
                IsActive = true
            };


            if (room.Machines.Count==0)
            {

                List<RoomAndMachine> roomsAndMachines = roomsAndMachinesRepository.GetRoomsAndMachinesByRoomId(room.Id);
                foreach (RoomAndMachine rm in roomsAndMachines)
                {
                    roomsAndMachinesRepository.DeleteConnection(rm.Id);
                }
            }
            else
            {
                foreach (MachineForRoom mr in room.Machines)
                {
                    
                    if (!context.Set<Machine>().Any(m => m.Id == mr.Id))
                        throw new Exception($"Az {mr.Id} azonosítójú gép nem létezik.");
                }

                List<RoomAndMachine>? roomsAndMachines = roomsAndMachinesRepository.GetRoomsAndMachinesByRoomId(room.Id);
                List<MachineForRoom> machines = room.Machines;
                if (roomsAndMachines!=null || roomsAndMachines!.Count>0)
                {
                    foreach (MachineForRoom mr in machines)
                    {
                        RoomAndMachine? roomAndMachine = roomsAndMachines.SingleOrDefault(rm => rm.MachineId == mr.Id && rm.RoomId==room.Id);
                        if (roomAndMachine == null)
                        {
                            RoomAndMachine newRm = new RoomAndMachine() { MachineCount = mr.Count, MachineId = mr.Id, RoomId = room.Id };
                            roomsAndMachinesRepository.ConnectRoomAndMachine(newRm);
                        }
                        else
                        {
                            if (roomAndMachine.MachineCount!=mr.Count)
                            {
                                roomsAndMachinesRepository.DeleteConnection(roomAndMachine.Id);
                                var newConnection = roomsAndMachinesRepository.ConnectRoomAndMachine(new RoomAndMachine() {MachineId=mr.Id,RoomId=id,MachineCount=mr.Count });
                                //RoomAndMachine toRemove = roomsAndMachines.Single(rm => rm.MachineId == newConnection.MachineId);
                                //roomsAndMachines.Remove(toRemove);
                            }
                            RoomAndMachine toRemove = roomsAndMachines.Single(rm => rm.MachineId == mr.Id);
                            roomsAndMachines.Remove(toRemove);

                            //var res = roomsAndMachinesRepository.UpdateRoomAndMachineConnection(roomAndMachine.Id, new RoomAndMachineRequest() { Id = roomAndMachine.Id, MachineId = mr.Id, MachineCount=mr.Count,RoomId=room.Id });

                        }

                    }
                    foreach (RoomAndMachine rm in roomsAndMachines)
                    {
                        roomsAndMachinesRepository.DeleteConnection(rm.Id);
                    }

                }

            }
            List<Image> images = imagesRepository.UploadImagesToRoom(room.Images, modifiedRoom.Id);

            this.context.Entry(modifiedRoom).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            this.context.SaveChanges();

            return roomsAndMachinesRepository.GetRoomWithMachinesById(modifiedRoom.Id);
        }

        public Room? DeleteRoom(int id)
        {
            var roomToDelete = this.context.Set<Room>().SingleOrDefault(r => r.Id == id);
            if (roomToDelete == null)
                return null;
            if (!roomToDelete.IsActive)
                return null;
            //List<RoomAndMachine> machinesInTheRoom = roomsAndMachinesRepository.GetRoomsAndMachinesByRoomId(roomToDelete.Id);
            //foreach (RoomAndMachine rm in machinesInTheRoom)
            //{
            //    roomsAndMachinesRepository.DeleteConnection(rm.Id);
            //}
            if (context.Set<Training>().Any(t=>t.RoomId == id))
            {
                roomToDelete.IsActive = false;
                this.context.Entry(roomToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                this.context.SaveChanges();
                return roomToDelete;

            }
            this.context.Set<Room>().Remove(roomToDelete);
            this.context.SaveChanges();
            return roomToDelete;
        }
    }
}
