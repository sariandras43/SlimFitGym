using SlimFitGym.EFData.Interfaces;
using SlimFitGym.Models.Models;
using SlimFitGym.Models.Requests;
using SlimFitGym.Models.Responses;
using SlimFitGymBackend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.EFData.Repositories
{
    public class ReservationRepository: IReservationRepository
    {
        readonly SlimFitGymContext context;
        readonly TokenGenerator tokenGenerator;
        readonly IAccountRepository accountRepository;

        public ReservationRepository(SlimFitGymContext context, TokenGenerator tokenGenerator, IAccountRepository accountRepository)
        {
            this.context = context;
            this.tokenGenerator = tokenGenerator;
            this.accountRepository = accountRepository;
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
        public List<Reservation>? GetReservationsByTrainingId(int trainingId)
        {
            if (trainingId <= 0)
                throw new Exception("Érvénytelen azonosító.");
            var res = context.Set<Reservation>().Where(r => r.TrainingId == trainingId).ToList();
            if (res != null)
                return res;
            return null;
        }
        public List<Reservation> GetReservationsByAccountId(int accountId)
        {
            if (accountId <= 0)
                throw new Exception("Érvénytelen azonosító.");
            var res = context.Set<Reservation>().Where(r => r.AccountId == accountId).ToList();
            return res;

        }

        public ReservationResponse? NewReservation(string token, ReservationRequest reservation)
        {
            if (reservation==null)
                throw new Exception("Érvénytelen lekérdezés.");
            if (reservation.TrainingId<=0)
                throw new Exception("Nincs ilyen edzés.");
            if (reservation.AccountId <= 0)
                throw new Exception("Nincs ilyen edző.");

            Account? accountFromToken = accountRepository.GetAccountById(tokenGenerator.GetAccountIdFromToken(token));
            if (accountFromToken == null)
                throw new Exception("Érvénytelen token.");
            if (reservation.AccountId != tokenGenerator.GetAccountIdFromToken(token))
                throw new UnauthorizedAccessException();


            Training? training = context.Set<Training>().SingleOrDefault(t=>t.Id == reservation.TrainingId && t.IsActive && t.TrainingStart > DateTime.UtcNow);
            if (training == null)
                return null;
            Account? trainer = context.Set<Account>().SingleOrDefault(a=>a.Id == training.TrainerId);
            // In theory, this check is not mandatory
            if (trainer == null)
                throw new Exception("Nincs ilyen edző.");
            if (trainer.Id == reservation.AccountId)
                throw new Exception("Az edzés szervezője nem iratkozhat fel a saját programjára.");
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
            Reservation? reservationToDelete = this.context.Set<Reservation>().SingleOrDefault(r => r.Id == id);
            if (reservationToDelete == null)
                return null;
            Training? training = this.context.Set<Training>().SingleOrDefault(t=>t.Id == reservationToDelete.Id && t.IsActive && t.TrainingStart> DateTime.UtcNow);
            if (training == null)
                throw new Exception("Nem lehet leiratkozni kitörölt vagy múltbéli edzésről.");
            this.context.Set<Reservation>().Remove(reservationToDelete);
            this.context.SaveChanges();
            return new ReservationResponse(reservationToDelete);
        }

        public ReservationResponse? DeleteReservationByTrainingAndAccountId(string token, int accountId, int trainingId)
        {
            if (accountId <= 0 ||trainingId <= 0)
                throw new Exception("Érvénytelen azonosítók.");

            Account? accountFromToken = accountRepository.GetAccountById(tokenGenerator.GetAccountIdFromToken(token));
            if (accountFromToken == null)
                throw new Exception("Érvénytelen token.");
            if (accountId != tokenGenerator.GetAccountIdFromToken(token))
                throw new UnauthorizedAccessException();

            Reservation? reservationToDelete = this.context.Set<Reservation>().SingleOrDefault(r => r.TrainingId == trainingId && r.AccountId==accountId);
            if (reservationToDelete == null)
                return null;
            Training? training = this.context.Set<Training>().SingleOrDefault(t => t.Id == reservationToDelete.TrainingId && t.IsActive && t.TrainingStart > DateTime.UtcNow);
            if (training == null)
                throw new Exception("Nem lehet leiratkozni kitörölt vagy múltbéli edzésről.");
            this.context.Set<Reservation>().Remove(reservationToDelete);
            this.context.SaveChanges();
            return new ReservationResponse(reservationToDelete);
        }
    }
}
