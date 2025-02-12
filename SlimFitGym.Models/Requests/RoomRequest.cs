using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Models.Requests
{
    public class RoomRequest
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int RecommendedPeople { get; set; }
        public List<MachineForRoom> Machines { get; set; }
    }
}
