using System;
using System.Collections.Generic;
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
    }
}
