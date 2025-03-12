using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimFitGym.Models.Models;

namespace SlimFitGym.Models.Responses
{
    public class MachineResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();
        public MachineResponse(Machine m, List<string> imageUrls) 
        {
            Id = m.Id;
            Name = m.Name;
            Description = m.Description;
            ImageUrls = imageUrls;
        }
        public MachineResponse()
        {
            
        }
    }
}
