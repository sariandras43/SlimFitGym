using SlimFitGym_Mobile.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SlimFitGym_Mobile.Services
{
    public static class AuthService
    {
        private static HttpClient _httpClient = new();
        private const string apiBaseURL = "http://10.0.2.2:8080/api/"; 
        public static event Action OnChange;

        public static async Task<LoginResult> Login(string email, string password)
        {
            try
            {
                var loginData = new { email = $"{email}", password = $"{password}" };
                var response = await _httpClient.PostAsync($"{apiBaseURL}auth/login",
                    new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json")
                );

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var account = JsonSerializer.Deserialize<AccountModel>(json, options);
                    return new LoginResult
                    {
                        Success = true,
                        Account = account,
                        ErrorMessage = string.Empty
                    };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var error = JsonSerializer.Deserialize<ErrorResult>(errorContent, options);
                    return new LoginResult
                    {
                        Success = false,
                        ErrorMessage = error.Message,
                        Account = null
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public static async Task<RegisterResult> Register(AccountModel newUser)
        {
            try
            {
                var registerData = new
                {
                    name = newUser.Name,
                    phone = newUser.Phone,
                    email = newUser.Email,
                    password = newUser.Password
                };

                var json = JsonSerializer.Serialize(registerData);
                var response = await _httpClient.PostAsync($"{apiBaseURL}auth/register",
                    new StringContent(json, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    return new RegisterResult
                    {
                        Success = true,
                        ErrorMessage = string.Empty
                    };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var error = JsonSerializer.Deserialize<ErrorResult>(errorContent, options);
                    return new RegisterResult
                    {
                        Success = false,
                        ErrorMessage = error.Message
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new RegisterResult
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        //public async Task<string> ResetPassword(string email, string newPassword, string newPasswordAgain)
        //{
        //    var response = await _httpClient.PostAsync($"{apiBaseURL}auth/resetpassword", new StringContent($"{{\"email\":\"{email}\", \"new password\":\"{newPassword}\", \"new password again\":\"{newPasswordAgain}\"}}", Encoding.UTF8, "application/json"));
        //    return await response.Content.ReadAsStringAsync();
        //}

        public static bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return System.Text.RegularExpressions.Regex.IsMatch(email, pattern);
        }

        public static bool IsValidPhoneNumber(string phone)
        {
            string pattern = @"^\+[1-9]\d{7,14}$";
            return System.Text.RegularExpressions.Regex.IsMatch(phone, pattern);
        }


        public static bool IsValidPassword(string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':\""\\|,.<>\/?]).{8,}$";
            return System.Text.RegularExpressions.Regex.IsMatch(password, pattern);
        }

        public static async Task SaveUser(AccountModel user)
        {
            try
            {
                AccountModel.LoggedInUser = user;
                var serializedUser = JsonSerializer.Serialize(user);
                await SecureStorage.SetAsync("LoggedInUser", serializedUser);
                Debug.WriteLine($"saved: {user.Name}");
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
