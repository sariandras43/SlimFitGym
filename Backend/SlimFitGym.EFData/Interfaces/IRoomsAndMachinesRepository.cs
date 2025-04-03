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
    public interface IRoomsAndMachinesRepository
    {
        List<RoomWithMachinesResponse>? GetRoomsWithMachines();
        RoomWithMachinesResponse? GetRoomWithMachinesById(int id);
        RoomAndMachineResponse? GetRoomAndMachineConnectionById(int id);
        List<RoomAndMachine>? GetRoomsAndMachinesByRoomId(int roomId);
        List<RoomWithMachinesResponse>? GetAllRoomsWithMachines();
        RoomAndMachine? GetRoomAndMachineByMachineAndRoomId(int machineId, int roomId);
        List<RoomAndMachine>? GetRoomAndMachineConnections();
        RoomAndMachineResponse? ConnectRoomAndMachine(RoomAndMachine roomAndMachine);
        RoomAndMachineResponse? UpdateRoomAndMachineConnection(int id, RoomAndMachineRequest rm);
        RoomAndMachineResponse? DeleteConnection(int id);
    }
}
