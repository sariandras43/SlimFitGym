using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimFitGym.Models.Models;

namespace SlimFitGym.Models.Requests
{
    public class ReservationRequest
    {
        public int Id { get; set; }
        public int TrainingId { get; set; }
        public int AccountId { get; set; }
        public ReservationRequest()
        {
            
        }
        public ReservationRequest(Reservation r)
        {
            Id = r.Id;
            TrainingId = r.TrainingId;
            AccountId = r.AccountId;
        }
    }
}
