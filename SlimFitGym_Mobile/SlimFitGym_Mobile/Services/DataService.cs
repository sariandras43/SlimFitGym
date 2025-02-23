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
        //    try
        //    {
        //        return await _httpClient.GetFromJsonAsync<List<MachineModel>>($"{apiBaseURL}machines");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public static async Task<List<TrainingModel>> GetTrainings()
        //{
        //    try
        //    {
        //        return await _httpClient.GetFromJsonAsync<List<TrainingModel>>($"{apiBaseURL}trainings");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public static async Task<List<TrainingModel>> GetSignedUpTrainings(int accountId)
        //{
        //    try
        //    {
        //        return await _httpClient.GetFromJsonAsync<List<TrainingModel>>($"{apiBaseURL}trainings/{accountId}");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        
        //public static async Task<TrainingModel> SignUpTraining(int accountId, int trainingId)
        ////{
        ////    try
        ////    {
        ////        var response = await _httpClient.PostAsJsonAsync($"{apiBaseURL}trainings/...");
        ////        return await response.Content.ReadFromJsonAsync<TrainingModel>();
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw new Exception(ex.Message);
        ////    }
        ////}

        //public static async Task<TrainingModel> DeleteSignUp(int accountId, int trainingId)
        //{
        //    try
        //    {
        //        var response = await _httpClient.DeleteAsync($"{apiBaseURL}trainings/....");
        //        return await response.Content.ReadFromJsonAsync<TrainingModel>();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public static async Task<TrainingModel> CreateTraining(TrainingModel training)
        //{
        //    try
        //    {
        //        var response = await _httpClient.PostAsJsonAsync($"{apiBaseURL}trainings", training);
        //        return await response.Content.ReadFromJsonAsync<TrainingModel>();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public static async Task<List<RoomModel>> GetRooms()
        //{
        //    try
        //    {
        //        return await _httpClient.GetFromJsonAsync<List<RoomModel>>($"{apiBaseURL}rooms");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public static async Task<List<PassModel>> GetPasses()
        //{
        //    try
        //    {
        //        return await _httpClient.GetFromJsonAsync<List<PassModel>>($"{apiBaseURL}passes");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public static async Task<List<PassModel>> GetAccountsPasses(int accountId)
        //{
        //    try
        //    {
        //        return await _httpClient.GetFromJsonAsync<List<PassModel>>($"{apiBaseURL}passes/{accountId}");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public static async Task<List<AccountModel>> GetAccount(int id)
        //{
        //    try
        //    {
        //        return await _httpClient.GetFromJsonAsync<List<AccountModel>>($"{apiBaseURL}accounts/{id}");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public static async Task<List<EntryModel>> GetEntriesOfAccount(int accountId)
        //{
        //    try
        //    {
        //        return await _httpClient.GetFromJsonAsync<List<EntryModel>>($"{apiBaseURL}entries/{accountId}");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public static async Task<EntryModel> PostEntry(EntryModel entry)
        //{
        //    try
        //    {
        //        var response = await _httpClient.PostAsJsonAsync($"{apiBaseURL}entries", entry);
        //        return await response.Content.ReadFromJsonAsync<EntryModel>();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}


    }


}
