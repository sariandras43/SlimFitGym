using SlimFitGym.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Models.Responses
{
    public class AccountResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Phone { get; set; }
        public DateTime ValidTo { get; set; }
        public string Role { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public AccountResponse(Account a, string token, DateTime validTo,string imageUrl="")
        {
            Id = a.Id;
            Name = a.Name;
            Email = a.Email;
            Token = token;
            Role = a.Role;
            Phone = a.Phone;
            ValidTo = validTo;
            IsActive = a.isActive;
            ImageUrl = imageUrl;
        }

        public AccountResponse(Account a, string imageUrl="")
        {
            Id = a.Id;
            Name = a.Name;
            Phone = a.Phone;
            Email = a.Email;
            Role = a.Role;
            ImageUrl = imageUrl;
            IsActive = a.isActive;
            Token = "";
        }

        public AccountResponse()
        {
            
        }
    }
}
