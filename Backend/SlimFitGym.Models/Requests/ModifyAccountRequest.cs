using SlimFitGym.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Models.Requests
{
    public class ModifyAccountRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string? NewPassword { get; set; }
        public string Image { get; set; }
        public ModifyAccountRequest(Account a)
        {
            Id = a.Id;
            Name = a.Name;
            Phone = a.Phone;
            Email = a.Email;
        }
        public ModifyAccountRequest()
        {

        }
    }
}
