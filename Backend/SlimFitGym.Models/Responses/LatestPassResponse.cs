using SlimFitGym.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Models.Responses
{
    public class LatestPassResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MaxEntries { get; set; }
        public int Days { get; set; }
        public decimal Price { get; set; }
        public bool isActive { get; set; }
        public bool isHighlighted { get; set; }
        public List<string> Benefits { get; set; } = new List<string>();
        public DateTime? ValidTo { get; set; }
        public int? RemainingEntries {  get; set; }
        public LatestPassResponse(PassResponse p, DateTime? validTo = null, int? remainingEntries = null)
        {
            Id = p.Id;
            Name = p.Name;
            Days = p.Days;
            MaxEntries = p.MaxEntries;
            Price = p.Price;
            isActive = p.isActive;
            isHighlighted = p.isHighlighted;
            Benefits = p.Benefits;
            ValidTo = validTo;
            RemainingEntries = remainingEntries;
            
        }
    }
}
