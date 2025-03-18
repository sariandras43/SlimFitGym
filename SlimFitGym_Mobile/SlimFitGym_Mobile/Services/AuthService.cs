using SlimFitGym_Mobile.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SlimFitGym_Mobile.Services
{
    public static class AuthService
    {
        private static HttpClient _httpClient = new();
        private const string apiBaseURL = "https://slimfitgymbackend-bdgbechedpcpaag4.westeurope-01.azurewebsites.net/api/";
        private static JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public static async Task<LoginResult> Login(string email, string password)
        {
            try
            {
                var loginData = new { email = $"{email}", password = $"{password}", rememberMe = true };
                var response = await _httpClient.PostAsync($"{apiBaseURL}auth/login",
                    new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json")
                );

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
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

        public static async Task<RegisterResult> Modify(AccountModel user)
        {
            SetBearerToken();
            try
            {
                var modifyData = new Dictionary<string, object>();
                    modifyData.Add("id", AccountModel.LoggedInUser.Id);
                if (user.Name != AccountModel.LoggedInUser.Name)
                    modifyData.Add("name", user.Name);
                if (user.Phone != AccountModel.LoggedInUser.Phone)
                    modifyData.Add("phone", user.Phone);
                if (user.Email != AccountModel.LoggedInUser.Email)
                    modifyData.Add("email", user.Email);
                if (user.Password != AccountModel.LoggedInUser.Password)
                    modifyData.Add("newPassword", user.Password);
                if (modifyData.Count == 0)
                    return new RegisterResult { Success = false, ErrorMessage = "Nincs megváltozott adat" };

                var json = JsonSerializer.Serialize(modifyData);
                var response = await _httpClient.PutAsync($"{apiBaseURL}auth/modify/{AccountModel.LoggedInUser.Id}",
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

        public static async Task SaveUser(AccountModel? oldUser, AccountModel newUser)
        {
            try
            {
                AccountModel.LoggedInUser = new AccountModel
                {
                    Id = newUser.Id,
                    Name = newUser.Name,
                    Email = newUser.Email,
                    Phone = newUser.Phone,
                    Role = newUser.Role,
                    Token = (oldUser != null) ? oldUser.Token : newUser.Token
                };
                await SecureStorage.SetAsync("LoggedInUser", JsonSerializer.Serialize(AccountModel.LoggedInUser));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving user: {ex.Message}");
            }
        }

        public static async Task LoadUser()
        {
            try
            {
                var savedUser = await SecureStorage.GetAsync("LoggedInUser");
                if (!string.IsNullOrEmpty(savedUser))
                {
                    AccountModel.LoggedInUser = JsonSerializer.Deserialize<AccountModel>(savedUser);
                    SetBearerToken();

                    var account = await _httpClient.GetFromJsonAsync<AccountModel>($"{apiBaseURL}auth/me");
                    if (account != null)
                    {
                        await SaveUser(AccountModel.LoggedInUser, account);
                    }
                }
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
