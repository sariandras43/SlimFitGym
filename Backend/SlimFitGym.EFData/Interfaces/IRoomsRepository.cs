using SlimFitGym.Models.Models;
using SlimFitGym.Models.Requests;
using SlimFitGym.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.EFData.Interfaces
{
    public interface IRoomsRepository
    {
        List<Room> GetAllRooms();
        Room? GetRoomById(int id);
        RoomWithMachinesResponse? NewRoom(RoomRequest newRoom);
        RoomWithMachinesResponse? UpdateRoom(int id, RoomRequest room);
        Room? DeleteRoom(int id);
    }
}
