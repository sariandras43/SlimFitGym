using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace SlimFitGym_Mobile.Models
{
    public class AccountModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool isActive { get; set; }
        public static AccountModel? LoggedInUser { get; set; } = null;

        public static List<AccountModel> users = new()
        {
            new AccountModel
            {
                Id = 1,
                Name = "John Doe",
                Email = "john.doe@gmail.com",
                Phone = "123",
                Password = "Tr4iner_",
                Role = "trainer",
                isActive = true
            },
            new AccountModel
            {
                Id = 2,
                Name = "Admin",
                Email = "admin@admin.com",
                Phone = "789",
                Password = "Admin1_",
                Role = "admin",
                isActive = true
            }
        };
    }
}
