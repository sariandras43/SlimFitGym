using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym_Mobile.Models
{
    public class PassModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? MaxEntries { get; set; }
        public int? RemainingEntries { get; set; }
        public int? Days { get; set; }
        public string? ValidTo { get; set; }
        public decimal Price { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsHighlighted { get; set; }
        public List<string>? Benefits { get; set; }
    }
}
