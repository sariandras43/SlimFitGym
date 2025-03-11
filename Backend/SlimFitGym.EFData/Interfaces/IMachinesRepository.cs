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
    public interface IMachinesRepository
    {
        List<MachineResponse> GetAllMachine();
        MachineResponse? GetMachineById(int id);
        MachineResponse? NewMachine(MachineRequest newMachine);
        MachineResponse? UpdateMachine(int id, MachineRequest machine);
        Machine? DeleteMachine(int id);
    }
}
