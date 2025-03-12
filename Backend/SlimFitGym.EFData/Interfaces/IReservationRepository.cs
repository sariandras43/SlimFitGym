using SlimFitGym.Models.Models;
using SlimFitGym.Models.Requests;
using SlimFitGym.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.EFData.Interfaces
{
    public interface IReservationRepository
    {
        List<ReservationResponse> GetAllReservations();
        ReservationResponse? GetReservationById(int id);
        List<Reservation>? GetReservationsByTrainingId(int trainingId);
        List<Reservation> GetReservationsByAccountId(int accountId);
        ReservationResponse? NewReservation(string token, ReservationRequest reservation);
        ReservationResponse? DeleteReservation(int id);
        ReservationResponse? DeleteReservationByTrainingAndAccountId(string token, int accountId, int trainingId);
    }
}
