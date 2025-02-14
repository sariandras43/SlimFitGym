using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SlimFitGym.Models.Models;
using SlimFitGym.Models.Responses;

namespace SlimFitGym.EFData.Repositories
{
    public class EntriesRepository
    {
        readonly SlimFitGymContext context;
        readonly AccountRepository accountRepository;
        readonly PurchasesRepository purchasesRepository;
        readonly PassesRepository passesRepository;
        public EntriesRepository(SlimFitGymContext slimFitGymContext, AccountRepository accountRepository, PurchasesRepository purchasesRepository, PassesRepository passesRepository)
        {
            this.context = slimFitGymContext;
            this.accountRepository = accountRepository;
            this.purchasesRepository = purchasesRepository;
            this.passesRepository = passesRepository;
        }

        public Entry? NewEntry(int accountId)
        {
            if (accountId<=0) throw new Exception("Ez a felhasználó nem létezik.");
            Account? account = accountRepository.GetAccountById(accountId);
            if (account==null) throw new Exception("Ez a felhasználó nem létezik.");
            Purchase? latestPurchase = purchasesRepository.GetLatestPurchaseByAccountId(accountId);
            if (latestPurchase == null) throw new Exception("Ez a felhasználó nem vett még bérletet.");
            Pass? pass = passesRepository.GetPassModelById(latestPurchase.PassId);
            if (pass == null)
                throw new Exception("Nem létezik ilyen bérlet.");
            if (pass.MaxEntries==0)
            {
                if ((DateTime.Now-latestPurchase.PurchaseDate).TotalSeconds<(latestPurchase.PurchaseDate.AddDays(pass.Days)-latestPurchase.PurchaseDate).TotalSeconds)
                    throw new Exception("A felhasználó legutóbb vásárolt bérlete nem érvényes már.");
                Entry entryToSave = new Entry() { AccountId=account.Id,EntryDate=DateTime.Now};
                Entry newEntry = context.Set<Entry>().Add(entryToSave).Entity;
                this.context.SaveChanges();
                return newEntry;
            }
            //TODO: Modify the pass model and checks at the repo
            //TODO: Modify the pass model and checks at the repo
            //TODO: Modify the pass model and checks at the repo
            else if (pass.MaxEntries>0 && pass.Days==0)
            {
                List<Entry> entriesByAccountId = GetEntriesByAccountId(accountId,latestPurchase.PurchaseDate.ToString());
                if (entriesByAccountId.Count == pass.MaxEntries) throw new Exception("Ezzel a bérlettel nem lehet többször belépni.");
                Entry entryToSave = new Entry() { AccountId = account.Id, EntryDate = DateTime.Now };
                Entry newEntry = context.Set<Entry>().Add(entryToSave).Entity;
                this.context.SaveChanges();
                return newEntry;
            }
            else
            {
                if ((DateTime.Now - latestPurchase.PurchaseDate).TotalSeconds < (latestPurchase.PurchaseDate.AddDays(pass.Days) - latestPurchase.PurchaseDate).TotalSeconds)
                    throw new Exception("A felhasználó legutóbb vásárolt bérlete nem érvényes már.");
                List<Entry> entriesByAccountId = GetEntriesByAccountId(accountId, latestPurchase.PurchaseDate.ToString());
                if (entriesByAccountId.Count == pass.MaxEntries) throw new Exception("Ezzel a bérlettel nem lehet többször belépni.");
                Entry entryToSave = new Entry() { AccountId = account.Id, EntryDate = DateTime.Now };
                Entry newEntry = context.Set<Entry>().Add(entryToSave).Entity;
                this.context.SaveChanges();
                return newEntry;

            }




        }

        public List<Entry> GetEntriesByAccountId(int accountId, string fromDate = "2025.01.01")
        {
            DateTime from;
            if (!DateTime.TryParse(fromDate,CultureInfo.InvariantCulture,out from))
                throw new Exception("Nem magyar formátumú a dátum.");
            return context.Set<Entry>().Where(e=>e.AccountId==accountId && e.EntryDate>from).ToList();
        }
    }
}
