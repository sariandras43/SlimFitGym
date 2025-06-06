﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Models.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [Required, ForeignKey("Trainings")]
        public int TrainingId { get; set; }

        [Required, ForeignKey("Accounts")]
        public int AccountId { get; set; }

        public Account Account { get; set; }
        public Training Training { get; set; }
    }
}
