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
    public interface IPassesRepository
    {
        List<PassResponse> GetAllActivePasses();
        List<PassResponse> GetAllPasses();
        PassResponse? GetOnlyActivePassById(int id);
        PassResponse? GetPassById(int id);
        Pass? GetPassModelById(int id);
        LatestPassResponse? GetLatestPassByAccountId(string token, int accountId);
        PassResponse? NewPass(PassRequest pass);
        PassResponse? UpdatePass(int id, PassRequest pass);
        PassResponse? DeleteOrMakePassInactive(int id);
    }
}

