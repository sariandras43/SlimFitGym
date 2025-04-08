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
    public interface ITrainingsRepository
    {
        List<Training> GetAllTrainings();
        List<TrainingResponse> GetActiveTrainings(string query = "", int limit = 20, int offset = 0);
        int GetTotalTrainingCountFromNow();
        List<TrainingResponse>? GetTrainingsByAccountId(string token, int accountId);
        TrainingResponse? GetActiveTraningById(int id);
        List<TrainingResponse>? GetActiveTrainingsByRoomId(int roomId);
        Training? GetTrainingById(int id);
        List<Training> FilterTrainings(string nameFragment, int limit, int offset);
        Training? NewTraining(string token, TrainingRequest training);
        Training? UpdateTraining(string token, int id, TrainingRequest training);
        TrainingResponse? DeleteOrMakeInactive(string token, int id);
        List<TrainingResponse>? GetActiveTrainingsByTrainerId(int trainerId);
    }
}
