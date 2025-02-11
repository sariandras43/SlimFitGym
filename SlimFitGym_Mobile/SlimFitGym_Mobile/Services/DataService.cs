using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using SlimFitGym_Mobile.Models;

namespace SlimFitGym_Mobile.Services
{
    public class DataService
    {
        private readonly HttpClient _httpClient;
        private const string apiBaseURL = "backendurl/api/";

        public DataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //public async Task<List<MachineModel>> GetMachines()
        //{
        //    return await _httpClient.GetFromJsonAsync<List<MachineModel>>($"{apiBaseURL}machines") ?? new List<MachineModel>();
        //}

        //public async Task<List<TrainingModel>> GetTrainings()
        //{
        //    return await _httpClient.GetFromJsonAsync<List<TrainingModel>>($"{apiBaseURL}trainings") ?? new List<TrainingModel>();
        //}

        //public async Task<List<TrainingModel>> GetSignedUpTrainings(string accountId)
        //{
        //    return await _httpClient.GetFromJsonAsync<List<TrainingModel>>($"{apiBaseURL}trainings/{accountId}") ?? new List<TrainingModel>();
        //}

        //public async Task<TrainingModel> CreateTraining(TrainingModel training)
        //{
        //    var response = await _httpClient.PostAsJsonAsync($"{apiBaseURL}trainings", training);
        //    return await response.Content.ReadFromJsonAsync<TrainingModel>();
        //}

        //public async Task<List<RoomModel>> GetRooms()
        //{
        //    return await _httpClient.GetFromJsonAsync<List<RoomModel>>($"{apiBaseURL}rooms") ?? new List<RoomModel>();
        //}

        //public async Task<List<PassModel>> GetPasses()
        //{
        //    return await _httpClient.GetFromJsonAsync<List<PassModel>>($"{apiBaseURL}passes") ?? new List<PassModel>();
        //}

        //public async Task<List<PassModel>> GetAccountsPasses(string accountId)
        //{
        //    return await _httpClient.GetFromJsonAsync<List<PassModel>>($"{apiBaseURL}passes/{accountId}") ?? new List<PassModel>();
        //}

        //public async Task<List<AccountModel>> GetAccount(int id)
        //{
        //    return await _httpClient.GetFromJsonAsync<List<AccountModel>>($"{apiBaseURL}accounts/{id}") ?? new List<AccountModel>();
        //}

        //public async Task<List<EntryModel>> GetEntries()
        //{
        //    return await _httpClient.GetFromJsonAsync<List<EntryModel>>($"{apiBaseURL}entries") ?? new List<EntryModel>();
        //}

        //public async Task<EntryModel> PostEntry(EntryModel entry)
        //{
        //    var response = await _httpClient.PostAsJsonAsync($"{apiBaseURL}entries", entry);
        //    return await response.Content.ReadFromJsonAsync<EntryModel>();
        //}

    }


}
