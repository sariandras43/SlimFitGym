using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym_Mobile.Models
{
    public class RoomModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RecommendedPeople { get; set; }

        public static List<RoomModel> GetRooms()
        {
            var rooms = new List<RoomModel>();
            rooms.Add(new RoomModel { Id = 1, Name = "Room 1", Description = "fdghfhdgfsfasdgas", RecommendedPeople = 10 });
            rooms.Add(new RoomModel { Id = 2, Name = "Room 2", Description = "fdghfhdgfsfasdgas", RecommendedPeople = 20 });
            rooms.Add(new RoomModel { Id = 3, Name = "Room 3", Description = "fdghfhdgfsfasdgas", RecommendedPeople = 15 });
            rooms.Add(new RoomModel { Id = 4, Name = "Room 4", Description = "fdghfhdgfsfasdgas", RecommendedPeople = 40 });
            rooms.Add(new RoomModel { Id = 5, Name = "Room 5", Description = "fdghfhdgfsfasdgas", RecommendedPeople = 30 });
            rooms.Add(new RoomModel { Id = 6, Name = "Room 6", Description = "fdghfhdgfsfasdgas", RecommendedPeople = 50 });
            rooms.Add(new RoomModel { Id = 7, Name = "Room 7", Description = "fdghfhdgfsfasdgas", RecommendedPeople = 15 });
            rooms.Add(new RoomModel { Id = 8, Name = "Room 8", Description = "fdghfhdgfsfasdgas", RecommendedPeople = 19 });
            rooms.Add(new RoomModel { Id = 9, Name = "Room 9", Description = "fdghfhdgfsfasdgas", RecommendedPeople = 25 });
            return rooms;
        }
    }
}
