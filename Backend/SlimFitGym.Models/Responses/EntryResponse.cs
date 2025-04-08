using SlimFitGym.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Models.Responses
{
    public class EntryResponse
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public DateTime EntryDate { get; set; }
        public string Name{ get; set; }

        public EntryResponse()
        {
            
        }

        public EntryResponse(Account account, Entry entry)
        {
            Id = entry.Id;
            AccountId = account.Id;
            EntryDate=entry.EntryDate;
            Name = account.Name;
        }
    }
}
