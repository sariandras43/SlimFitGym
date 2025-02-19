using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym_Mobile.Models
{
    public class EntryModel
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public DateTime EntryDate { get; set; }

        public static List<EntryModel> lastEntriesOfAccount = new()
        {
            new EntryModel { Id = 1, EntryDate = DateTime.Now },
            new EntryModel { Id = 2, EntryDate = DateTime.Now },
            new EntryModel { Id = 3, EntryDate = DateTime.Now },
            new EntryModel { Id = 4, EntryDate = DateTime.Now },
            new EntryModel { Id = 5, EntryDate = DateTime.Now }
        };
    }
}
