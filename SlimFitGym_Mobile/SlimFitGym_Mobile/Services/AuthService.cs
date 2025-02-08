using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym_Mobile.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private const string apiBaseURL = "backendurl/api/";

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //public async Task<string> Login(string email, string password)
        //{
        //    var response = await _httpClient.PostAsync($"{apiBaseURL}auth/login", new StringContent($"{{\"email\":\"{email}\",\"password\":\"{password}\"}}", Encoding.UTF8, "application/json"));
        //    return await response.Content.ReadAsStringAsync();
        //}

        //public async Task<string> Register(string name, string email, string password, string passwordAgain)
        //{
        //    var response = await _httpClient.PostAsync($"{apiBaseURL}auth/register", new StringContent($"{{\"name\":\"{name}\",\"email\":\"{email}\",\"password\":\"{password}\",\"password again\":\"{passwordAgain}\"}}", Encoding.UTF8, "application/json"));
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
            // - Minimum 8 characters
            // - At least one uppercase letter
            // - At least one lowercase letter
            // - At least one digit
            // - At least one special character
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':\""\\|,.<>\/?]).{8,}$";
            return System.Text.RegularExpressions.Regex.IsMatch(password, pattern);
        }
    }
}
