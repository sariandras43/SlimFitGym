using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
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
        public const string apiBaseURL = "https://slimfitgymbackend-bdgbechedpcpaag4.westeurope-01.azurewebsites.net/api/";

        public static async Task<List<MachineModel>> GetMachines()
        {
            SetBearerToken();
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
            SetBearerToken();
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
            SetBearerToken();
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
            SetBearerToken();
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
            SetBearerToken();
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

        public static async Task SignUpTraining(int accountId, int trainingId)
        {
            SetBearerToken();
            try
            {
                var newSignUp = new
                {
                    trainingId = trainingId,
                    accountId = accountId
                };
                var json = JsonSerializer.Serialize(newSignUp);
                var response = await _httpClient.PostAsync($"{apiBaseURL}trainings/signup",
                    new StringContent(json, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return;
                }
                else
                {
                    throw new Exception("Hiba az edzésre jelentkezés során!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteSignUp(int accountId, int trainingId)
        {
            SetBearerToken();
            try
            {
                var signUp = new
                {
                    trainingId = trainingId,
                    accountId = accountId
                };
                var json = JsonSerializer.Serialize(signUp);
                var response = await _httpClient.PostAsync($"{apiBaseURL}trainings/signout",
                    new StringContent(json, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return;
                }
                else
                {
                    throw new Exception("Hiba a leiratkozás során!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<AddTrainingResult> CreateTraining(TrainingModel training)
        {
            SetBearerToken();
            try
            {
                var newTraining = new
                {
                    id = training.Id,
                    trainerId = training.TrainerId,
                    roomId = training.RoomId,
                    name = training.Name,
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
            SetBearerToken();
            try
            {
                var updatedTraining = new
                {
                    id = training.Id,
                    trainerId = training.TrainerId,
                    roomId = training.RoomId,
                    name = training.Name,
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
            SetBearerToken();
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
            SetBearerToken();
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
            SetBearerToken();
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
            SetBearerToken();
            try
            {
                var pass = await _httpClient.GetFromJsonAsync<PassModel>($"{apiBaseURL}passes/accounts/{accountId}/latest");
                return pass ?? new PassModel();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<List<EntryModel>> GetEntriesOfAccount(int accountId)
        {
            SetBearerToken();
            try
            {
                //wrong
                var entries = await _httpClient.GetFromJsonAsync<List<EntryModel>>($"{apiBaseURL}entries/{accountId}/limit=5=/offset=0");
                return entries ?? new List<EntryModel>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<EntryModel> PostEntry(EntryModel entry)
        {
            SetBearerToken();
            try
            {
                var newEntry = new
                {
                    accountId = entry.AccountId,
                    entryTime = entry.EntryDate
                };
                var json = JsonSerializer.Serialize(newEntry);
                var response = await _httpClient.PostAsync(
                    $"{apiBaseURL}entries/{entry.AccountId}",
                    new StringContent(json, Encoding.UTF8, "application/json")
                );
                return await response.Content.ReadFromJsonAsync<EntryModel>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void SetBearerToken()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccountModel.LoggedInUser.Token);
        }
    }
}
