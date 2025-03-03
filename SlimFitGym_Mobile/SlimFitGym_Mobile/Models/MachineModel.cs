using SlimFitGym_Mobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym_Mobile.Models
{
    public class MachineModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } = null;
        public string[]? ImageUrls { get; set; } = null;
    }
}
