using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Models.Requests
{
    public class PassRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MaxEntries { get; set; }
        public decimal Price { get; set; }
        public int Days { get; set; }
        public bool isActive { get; set; } = true;
        public bool isHighlighted { get; set; } = false;
        public List<string> Benefits { get; set; } = new List<string>();
    }
}
