using SlimFitGym.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Models.Responses
{
    public class PassResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MaxEntries { get; set; }
        public decimal Price { get; set; }
        public bool isActive { get; set; }
        public bool isHighlighted { get; set; }
        public List<string> Benefits { get; set; } = new List<string>();

        public PassResponse(Pass p)
        {
            Id = p.Id;
            Name = p.Name;
            MaxEntries = p.MaxEntries;
            Price = p.Price;
            isActive = p.IsActive;
            isHighlighted = p.IsHighlighted;
        }
    }
}
