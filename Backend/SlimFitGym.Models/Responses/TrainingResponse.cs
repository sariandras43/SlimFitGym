using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Models.Responses
{
    public class TrainingResponse
    {
        public int Id { get; set; }
        public string Trainer { get; set; }
        public string Room { get; set; }
        public string Name { get; set; }
        public DateTime TrainingStart { get; set; }
        public DateTime TrainingEnd { get; set; }
        public int MaxPeople { get; set; }
        public int FreePlaces { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
