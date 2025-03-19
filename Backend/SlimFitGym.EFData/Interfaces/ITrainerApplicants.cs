using SlimFitGym.Models.Models;
using SlimFitGym.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.EFData.Interfaces
{
    public interface ITrainerApplicantsRepository
    {
        List<TrainerApplicant> GetAllApplicants();
        TrainerApplicant? GetApplicantById(int id);
        TrainerApplicant? NewApplicant(string token, int accountId);
        AccountResponse? AcceptAsTrainer(int id);
        AccountResponse? Reject(int id);
    }
}
