﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Models.Responses
{
    public class RoomWithMachinesResponse
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public int RecommendedPeople { get; set; }
        public required string Name { get; set; }
        public bool IsActive { get; set; }
        public List<MachineDetails> Machines { get; set; } = new List<MachineDetails>();
        public required string ImageUrl { get; set; }
    }
    public class MachineDetails
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int MachineCount { get; set; }
        public required List<string> ImageUrls { get; set; }
    }
}
