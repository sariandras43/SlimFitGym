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
    public class PurchasesRepository
    {
        readonly SlimFitGymContext context;
        readonly AccountRepository accountRepository;

        public PurchasesRepository(SlimFitGymContext context, AccountRepository accountRepository)
        {
            this.context = context;
            this.accountRepository = accountRepository;
        }


        public List<PurchaseResponse> GetAllPurchases() 
        {
            return context.Set<Purchase>().Select(p=>new PurchaseResponse(p)).ToList();
        }

        public List<Purchase>? GetPurchasesByAccountId(int accountId)
        {
            if (accountId<= 0) return null;

            List<Purchase>? purchases = context.Set<Purchase>().Where(p => p.AccountId == accountId).ToList();
            if (purchases == null) return null;
            return purchases;
        }

        public Purchase? GetLatestPurchaseByAccountId(int accountId)
        {
            if (accountId <= 0) return null;

            Purchase? purchase = context.Set<Purchase>().Where(p => p.AccountId == accountId).OrderByDescending(p=>p.Id).FirstOrDefault();
            if (purchase == null) return null;
            return purchase;
        }

        public PurchaseResponse? GetPurchaseById(int id)
        {
            if (id <= 0)
                return null;
            var res = context.Set<Purchase>().ToList().SingleOrDefault(t => t.Id == id);
            if (res != null)
                return new PurchaseResponse(res);
            return null;
        }

        public PurchaseResponse? NewPurchase(PurchaseRequest purchase)
        {
            if (purchase.PassId <= 0)
                throw new Exception("Ilyen bérlet nem létezik.");
            if (purchase.AccountId <= 0)
                throw new Exception("Ilyen felhasználó nem létezik.");
            Pass? p = context.Set<Pass>().SingleOrDefault(p => p.Id == purchase.PassId);
            if (p == null)
                throw new Exception("Ilyen bérlet nem létezik.");
            if (!p.IsActive)
                throw new Exception("Ilyen bérlet nem létezik.");
            Account? a = context.Set<Account>().SingleOrDefault(a => a.Id == purchase.AccountId);
            if (a == null)
                throw new Exception("Ilyen felhasználó nem létezik.");
            if (!a.isActive)
                throw new Exception("Ilyen felhasználó nem létezik.");

            Purchase savedPurchase = this.context.Set<Purchase>().Add(new Purchase() { AccountId=purchase.AccountId,PassId=purchase.PassId,PurchaseDate=DateTime.Now}).Entity;

            this.context.SaveChanges();
            return new PurchaseResponse(savedPurchase);
        }
    }
}
