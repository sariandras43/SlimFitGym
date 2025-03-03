using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SlimFitGym_Mobile.Models;

namespace SlimFitGym_Mobile.Services
{
    public static class DataService
    {
        private static HttpClient _httpClient = new();
        public const string apiBaseURL = "http://10.0.2.2:8080/api/";

        public static async Task<List<MachineModel>> GetMachines()
        {
            try
            {
                var machines = await _httpClient.GetFromJsonAsync<List<MachineModel>>($"{apiBaseURL}machines");
                return machines ?? new List<MachineModel>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<MachineModel> GetMachine(int machineId)
        {
            try
            {
                var machine = await _httpClient.GetFromJsonAsync<MachineModel>($"{apiBaseURL}machines/{machineId}");
                return machine ?? new MachineModel();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<List<TrainingModel>> GetTrainings()
        {
            try
            {
                var trainings = await _httpClient.GetFromJsonAsync<List<TrainingModel>>($"{apiBaseURL}trainings");
                return trainings ?? new List<TrainingModel>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public static async Task<TrainingModel> GetTraining(int trainingId)
        {
            try
            {
                var training = await _httpClient.GetFromJsonAsync<TrainingModel>($"{apiBaseURL}trainings/{trainingId}");
                return training;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<List<TrainingModel>> GetSignedUpTrainings(int accountId)
        {
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var signedUpTrainings = await _httpClient.GetFromJsonAsync<List<TrainingModel>>($"{apiBaseURL}trainings/account/{accountId}", options);
                return signedUpTrainings ?? new List<TrainingModel>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<TrainingModel> SignUpTraining(int accountId, int trainingId)
        {
            try
            {
                var response = await _httpClient.PostAsync($"{apiBaseURL}trainings/...", null);
                return await response.Content.ReadFromJsonAsync<TrainingModel>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<TrainingModel> DeleteSignUp(int accountId, int trainingId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{apiBaseURL}trainings/...");
                return await response.Content.ReadFromJsonAsync<TrainingModel>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<AddTrainingResult> CreateTraining(TrainingModel training)
        {
            try
            {
                var newTraining = new
                {
                    id = training.Id,
                    trainerId = training.TrainerId,
                    roomId = training.RoomId,
                    name = training.Name,
                    description = training.Description,
                    trainingStart = training.TrainingStart,
                    trainingEnd = training.TrainingEnd,
                    maxPeople = training.MaxPeople
                };
                var json = JsonSerializer.Serialize(newTraining);
                var response = await _httpClient.PostAsync(
                    $"{apiBaseURL}trainings",
                    new StringContent(json, Encoding.UTF8, "application/json")
                );
                if (response.IsSuccessStatusCode)
                {
                    return new AddTrainingResult
                    {
                        Success = true,
                        ErrorMessage = string.Empty
                    };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var error = JsonSerializer.Deserialize<ErrorResult>(errorContent, options);
                    return new AddTrainingResult
                    {
                        Success = false,
                        ErrorMessage = error?.Message ?? "Hiba az edzés létrehozása során"
                    };
                }
            }
            catch (Exception ex)
            {
                return new AddTrainingResult
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public static async Task<AddTrainingResult> UpdateTraining(TrainingModel training)
        {
            try
            {
                var updatedTraining = new
                {
                    id = training.Id,
                    trainerId = training.TrainerId,
                    roomId = training.RoomId,
                    name = training.Name,
                    description = training.Description,
                    trainingStart = training.TrainingStart,
                    trainingEnd = training.TrainingEnd,
                    maxPeople = training.MaxPeople
                };
                var json = JsonSerializer.Serialize(updatedTraining);
                var response = await _httpClient.PutAsync(
                    $"{apiBaseURL}trainings/{training.Id}",
                    new StringContent(json, Encoding.UTF8, "application/json")
                );
                if (response.IsSuccessStatusCode)
                {
                    return new AddTrainingResult
                    {
                        Success = true,
                        ErrorMessage = string.Empty
                    };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var error = JsonSerializer.Deserialize<ErrorResult>(errorContent, options);
                    return new AddTrainingResult
                    {
                        Success = false,
                        ErrorMessage = error?.Message ?? "Hiba az edzés módosítása során"
                    };
                }
            }
            catch (Exception ex)
            {
                return new AddTrainingResult
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public static async Task<TrainingModel> DeleteTraining(int trainingId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{apiBaseURL}trainings/{trainingId}");
                return await response.Content.ReadFromJsonAsync<TrainingModel>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<List<RoomModel>> GetRooms()
        {
            try
            {
                var rooms = await _httpClient.GetFromJsonAsync<List<RoomModel>>($"{apiBaseURL}rooms");
                return rooms ?? new List<RoomModel>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<List<PassModel>> GetPasses()
        {
            try
            {
                var passes = await _httpClient.GetFromJsonAsync<List<PassModel>>($"{apiBaseURL}passes");
                return passes ?? new List<PassModel>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<PassModel> GetAccountsPass(int accountId)
        {
            try
            {
                var pass = await _httpClient.GetFromJsonAsync<PassModel>($"{apiBaseURL}passes/{accountId}");
                return pass ?? new PassModel();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<List<EntryModel>> GetEntriesOfAccount(int accountId)
        {
            try
            {
                var entries = await _httpClient.GetFromJsonAsync<List<EntryModel>>($"{apiBaseURL}entries/{accountId}");
                return entries ?? new List<EntryModel>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<EntryModel> PostEntry(EntryModel entry)
        {
            try
            {
                var newEntry = new
                {
                    accountId = entry.AccountId,
                    entryTime = entry.EntryDate
                };
                var json = JsonSerializer.Serialize(newEntry);
                var response = await _httpClient.PostAsync(
                    $"{apiBaseURL}entries",
                    new StringContent(json, Encoding.UTF8, "application/json")
                );
                return await response.Content.ReadFromJsonAsync<EntryModel>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
