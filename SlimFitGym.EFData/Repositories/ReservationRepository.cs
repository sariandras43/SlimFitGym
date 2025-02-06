using SlimFitGym.Models.Models;
using SlimFitGym.Models.Requests;
using SlimFitGym.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.EFData.Repositories
{
    public class ReservationRepository
    {
        readonly SlimFitGymContext context;

        public ReservationRepository(SlimFitGymContext context)
        {
            this.context = context;
        }

        public List<ReservationResponse> GetAllReservations()
        {
            return this.context.Set<Reservation>().Select(r=>new ReservationResponse(r)).ToList();
        }

        public ReservationResponse? GetReservationById(int id)
        {
            if (id <= 0)
                throw new Exception("Érvénytelen azonosító.");
            var res = context.Set<Reservation>().SingleOrDefault(r => r.Id == id);
            if (res != null)
                return new ReservationResponse(res);
            return null;
        }

        public ReservationResponse? NewReservation(ReservationRequest reservation)
        {
            if (reservation==null)
                throw new Exception("Érvénytelen lekérdezés.");
            if (reservation.TrainingId<=0)
                throw new Exception("Nincs ilyen edzés.");
            if (reservation.AccountId <= 0)
                throw new Exception("Nincs ilyen edző.");
            Training? training = context.Set<Training>().SingleOrDefault(t=>t.Id == reservation.TrainingId);
            if (training == null)
                throw new Exception("Nincs ilyen edzés.");
            Account? trainer = context.Set<Account>().SingleOrDefault(a=>a.Id == training.TrainerId);
            // In theory, this check is not mandatory
            if (trainer == null)
                throw new Exception("Nincs ilyen edző.");
            if (trainer.Role == "user")
                throw new Exception("Nem lehet nála edzést felvenni.");
            if (context.Set<Reservation>().Any(r => r.AccountId == reservation.AccountId && r.TrainingId == reservation.TrainingId))
                throw new Exception("Ez a felhasználó már be van iratkozva erre az edzésre.");
            int numberOfPeopleOnSpecificTraining = context.Set<Reservation>().Where(r=>r.TrainingId==reservation.TrainingId).Count();
            if (numberOfPeopleOnSpecificTraining >= training.MaxPeople)
                throw new Exception("Beteltek a helyek ezen az edzésen.");

            Reservation savedReservation = this.context.Set<Reservation>().Add(new Reservation() { Id=reservation.Id,AccountId=reservation.AccountId,TrainingId=reservation.TrainingId}).Entity;
            this.context.SaveChanges();
            return new ReservationResponse(reservation);
        }

        public ReservationResponse? DeleteReservation(int id)
        {
            if (id <= 0)
                throw new Exception("Érvénytelen azonosító.");
            Reservation? reservationToDelete = this.context.Set<Reservation>().SingleOrDefault(t => t.Id == id);
            if (reservationToDelete == null)
                return null;

            this.context.Set<Reservation>().Remove(reservationToDelete);
            this.context.SaveChanges();
            return new ReservationResponse(reservationToDelete);
        }
    }
}
