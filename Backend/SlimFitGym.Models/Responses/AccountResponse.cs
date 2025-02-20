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
        public DateTime ValidTo { get; set; }
        public string Role { get; set; }
        public AccountResponse(Account a, string token)
        {
            Id = a.Id;
            Name = a.Name;
            Email = a.Email;
            Token = token;
            Role = a.Role;
            ValidTo = DateTime.Now.AddDays(1);
        }

        public AccountResponse(Account a)
        {
            Id = a.Id;
            Name = a.Name;
            Email = a.Email;
            Token = "To be implemented";
            Role = a.Role;
            ValidTo = DateTime.Now.AddMinutes(2);
        }
        public AccountResponse(AccountResponse a)
        {
            Id = a.Id;
            Name = a.Name;
            Email = a.Email;
            Token = "To be implemented";
            Role = a.Role;
            ValidTo = DateTime.Now.AddMinutes(30);
        }
    }
}
