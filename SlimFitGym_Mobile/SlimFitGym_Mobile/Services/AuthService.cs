using SlimFitGym_Mobile.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SlimFitGym_Mobile.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private const string apiBaseURL = "backendurl/api/"; 
        public static event Action OnChange;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //public async Task<string> Login(string email, string password)
        //{
        //    var response = await _httpClient.PostAsync($"{apiBaseURL}auth/login", new StringContent($"{{\"email\":\"{email}\",\"password\":\"{password}\"}}", Encoding.UTF8, "application/json"));
        //    return await response.Content.ReadAsStringAsync();
        //}

        //public async Task<string> Register(AccountModel newUser, string passwordAgain)
        //{
        //    var response = await _httpClient.PostAsync($"{apiBaseURL}auth/register", new StringContent($"{{\"name\":\"{newUser.Name}\",\"email\":\"{newUser.Email}\",\"password\":\"{newUser.Password}\",\"password again\":\"{passwordAgain}\"}}", Encoding.UTF8, "application/json"));
        //    return await response.Content.ReadAsStringAsync();
        //}

        //public async Task<string> ResetPassword(string email, string newPassword, string newPasswordAgain)
        //{
        //    var response = await _httpClient.PostAsync($"{apiBaseURL}auth/resetpassword", new StringContent($"{{\"email\":\"{email}\", \"new password\":\"{newPassword}\", \"new password again\":\"{newPasswordAgain}\"}}", Encoding.UTF8, "application/json"));
        //    return await response.Content.ReadAsStringAsync();
        //}

        public bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return System.Text.RegularExpressions.Regex.IsMatch(email, pattern);
        }

        public bool IsValidPassword(string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':\""\\|,.<>\/?]).{8,}$";
            return System.Text.RegularExpressions.Regex.IsMatch(password, pattern);
        }

        public static async Task SaveUser(AccountModel user)
        {
            try
            {
                var serializedUser = JsonSerializer.Serialize(user);
                await SecureStorage.SetAsync("LoggedInUser", serializedUser);
                Debug.WriteLine($"saved: {user}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving user: {ex.Message}");
            }
        }

        public static async Task<AccountModel> LoadUser()
        {
            try
            {
                var userData = await SecureStorage.GetAsync("LoggedInUser");
                if (!string.IsNullOrEmpty(userData))
                {
                    Debug.WriteLine($"loaded: {userData}");
                    return JsonSerializer.Deserialize<AccountModel>(userData);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading user: {ex.Message}");
            }

            return null;
        }
    }
}
