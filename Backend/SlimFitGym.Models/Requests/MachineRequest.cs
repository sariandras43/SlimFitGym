using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Models.Requests
{
    public class MachineRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } = null;
        public List<ImageRequest> Images { get; set; }
    }
}
