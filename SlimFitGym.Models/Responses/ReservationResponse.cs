using SlimFitGym.Models.Models;
using SlimFitGym.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Models.Responses
{
    public class ReservationResponse
    {
        public ReservationResponse()
        {
            
        }

        public ReservationResponse(Reservation r)
        {
            Id = r.Id;
            TrainingId = r.TrainingId;
            AccountId = r.AccountId;
        }
        public ReservationResponse(ReservationRequest r)
        {
            Id = r.Id;
            TrainingId = r.TrainingId;
            AccountId = r.AccountId;
        }
        public int Id { get; set; }
        public int TrainingId { get; set; }
        public int AccountId { get; set; }
    }
}
