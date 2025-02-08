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
        public string AccountId { get; set; }
        public DateTime EntryDate { get; set; }
    }
}
