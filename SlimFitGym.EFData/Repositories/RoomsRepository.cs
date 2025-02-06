using Microsoft.EntityFrameworkCore;
using SlimFitGym.Models.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.EFData.Repositories
{
    public class RoomsRepository
    {
        readonly SlimFitGymContext context;

        public RoomsRepository(SlimFitGymContext context)
        {
            this.context = context;
        }

        public List<Room> GetAllRooms() 
        {
            return context.Set<Room>().ToList();
        }

        public Room? GetRoomById(int id)
        {
            Room? result = context.Set<Room>().ToList().SingleOrDefault(r => r.Id == id);
            if (result == null)
                return null;
            
            return result;
        }

        public Room? NewRoom(Room newRoom)
        {

            if (newRoom == null)
                throw new Exception("Hibás kérés.");
            if (newRoom.Name == null || newRoom.Name.Length == 0)
                throw new Exception("A név mező kitöltése kötelező.");
            if (newRoom.Description == null || newRoom.Description.Length == 0)
                throw new Exception("A leírás mező kitöltése kötelező.");
            if (newRoom.RecommendedPeople == 0)
                throw new Exception("A javasolt befogadóképesség mező kitöltése kötelező");
            if (newRoom.Name.Length > 100)
                throw new Exception("A név maximum 100 karakter hosszú lehet.");
            if (newRoom.Description != null && newRoom.Description!.Length > 500)
                throw new Exception("A leírás maximum 500 karakter hosszú lehet.");
            if (context.Set<Room>().Any(r => r.Name == newRoom.Name))
                throw new Exception("Ilyen terem már létezik.");


            Room savedRoom = this.context.Set<Room>().Add(newRoom).Entity;
            this.context.SaveChanges();
            return savedRoom;

        }

        public Room? UpdateRoom(int id,Room room)
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
            if (context.Set<Room>().Any(r => r.Name == room.Name))
                throw new Exception("Ilyen terem már létezik.");


            this.context.Entry(room).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            this.context.SaveChanges();
            return room;
        }

        public Room? DeleteRoom(int id)
        {
            var roomToDelete = this.context.Set<Room>().SingleOrDefault(r => r.Id == id);
            if (roomToDelete == null)
                return null;

            this.context.Set<Room>().Remove(roomToDelete);
            this.context.SaveChanges();
            return roomToDelete;
        }
    }
}
