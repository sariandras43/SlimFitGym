using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using SlimFitGym_Mobile.Models;

namespace SlimFitGym_Mobile.Services
{
    public static class DataService
    {
        private static HttpClient _httpClient = new();
        private const string apiBaseURL = "backendurl/api/";


        //public static async Task<List<MachineModel>> GetMachines()
        //{
        //    return await _httpClient.GetFromJsonAsync<List<MachineModel>>($"{apiBaseURL}machines") ?? new List<MachineModel>();
        //}

        //public static async Task<List<TrainingModel>> GetTrainings()
        //{
        //    return await _httpClient.GetFromJsonAsync<List<TrainingModel>>($"{apiBaseURL}trainings") ?? new List<TrainingModel>();
        //}

        //public static async Task<List<TrainingModel>> GetSignedUpTrainings(string accountId)
        //{
        //    return await _httpClient.GetFromJsonAsync<List<TrainingModel>>($"{apiBaseURL}trainings/{accountId}") ?? new List<TrainingModel>();
        //}

        //public static async Task<TrainingModel> CreateTraining(TrainingModel training)
        //{
        //    var response = await _httpClient.PostAsJsonAsync($"{apiBaseURL}trainings", training);
        //    return await response.Content.ReadFromJsonAsync<TrainingModel>();
        //}

        //public static async Task<List<RoomModel>> GetRooms()
        //{
        //    return await _httpClient.GetFromJsonAsync<List<RoomModel>>($"{apiBaseURL}rooms") ?? new List<RoomModel>();
        //}

        //public static async Task<List<PassModel>> GetPasses()
        //{
        //    return await _httpClient.GetFromJsonAsync<List<PassModel>>($"{apiBaseURL}passes") ?? new List<PassModel>();
        //}

        //public static async Task<List<PassModel>> GetAccountsPasses(string accountId)
        //{
        //    return await _httpClient.GetFromJsonAsync<List<PassModel>>($"{apiBaseURL}passes/{accountId}") ?? new List<PassModel>();
        //}

        //public static async Task<List<AccountModel>> GetAccount(int id)
        //{
        //    return await _httpClient.GetFromJsonAsync<List<AccountModel>>($"{apiBaseURL}accounts/{id}") ?? new List<AccountModel>();
        //}

        //public static async Task<List<EntryModel>> GetEntries()
        //{
        //    return await _httpClient.GetFromJsonAsync<List<EntryModel>>($"{apiBaseURL}entries") ?? new List<EntryModel>();
        //}

        public static async Task<EntryModel> PostEntry(EntryModel entry)
        {
            var response = await _httpClient.PostAsJsonAsync($"{apiBaseURL}entries", entry);
            return await response.Content.ReadFromJsonAsync<EntryModel>();
        }

    }


}
