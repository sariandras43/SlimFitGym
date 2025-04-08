using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using SlimFitGym.EFData.Interfaces;
using SlimFitGym.Models.Models;
using SlimFitGym.Models.Responses;
using SlimFitGymBackend;

namespace SlimFitGym.EFData.Repositories
{
    public class EntriesRepository:IEntriesRepository
    {
        readonly SlimFitGymContext context;
        readonly IAccountRepository accountRepository;
        readonly IPurchasesRepository purchasesRepository;
        readonly IPassesRepository passesRepository;
        readonly TokenGenerator tokenGenerator;
        public EntriesRepository(SlimFitGymContext slimFitGymContext, IAccountRepository accountRepository, IPurchasesRepository purchasesRepository, IPassesRepository passesRepository, TokenGenerator tokenGenerator)
        {
            this.context = slimFitGymContext;
            this.accountRepository = accountRepository;
            this.purchasesRepository = purchasesRepository;
            this.passesRepository = passesRepository;
            this.tokenGenerator = tokenGenerator;
        }

        public Entry NewEntry(string token,int accountId)
        {
            Account? accountFromToken = accountRepository.GetAccountById(tokenGenerator.GetAccountIdFromToken(token));
            if (accountFromToken == null)
                throw new Exception("Ércénytelen token.");
            if (accountFromToken.Role=="user" || accountFromToken.Role=="trainer")
                throw new UnauthorizedAccessException();
            if (accountId<=0) throw new Exception("Érvénytelen azonosító.");
            Account? account = accountRepository.GetAccountById(accountId);
            if (account==null) throw new Exception("Ez a felhasználó nem létezik.");
            if (account.Role=="trainer" || account.Role == "admin" || account.Role == "employee")
            {
                Entry entryToSave = new Entry() { AccountId=account.Id,EntryDate=DateTime.Now};
                Entry newEntry = context.Set<Entry>().Add(entryToSave).Entity;
                this.context.SaveChanges();
                return newEntry;
            }
            Purchase? latestPurchase = purchasesRepository.GetLatestPurchaseByAccountId(accountId);
            if (latestPurchase == null) throw new Exception("Ez a felhasználó nem vett még bérletet.");
            Pass? pass = passesRepository.GetPassModelById(latestPurchase.PassId);
            if (pass == null)
                throw new Exception("Nem létezik ilyen bérlet.");
            if (pass.MaxEntries==0)
            {
                if ((DateTime.Now-latestPurchase.PurchaseDate).TotalSeconds>(latestPurchase.PurchaseDate.AddDays(pass.Days)-latestPurchase.PurchaseDate).TotalSeconds)
                    throw new Exception("A felhasználó legutóbb vásárolt bérlete nem érvényes már.");
                Entry entryToSave = new Entry() { AccountId=account.Id,EntryDate=DateTime.Now};
                Entry newEntry = context.Set<Entry>().Add(entryToSave).Entity;
                this.context.SaveChanges();
                return newEntry;
            }
            else if (pass.MaxEntries>0 && pass.Days==0)
            {
                List<Entry> entriesByAccountId = GetEntriesByAccountId(token, accountId, latestPurchase.PurchaseDate.ToString(), pass.MaxEntries);
                if (entriesByAccountId.Count == pass.MaxEntries) throw new Exception("Ezzel a bérlettel nem lehet többször belépni.");
                Entry entryToSave = new Entry() { AccountId = account.Id, EntryDate = DateTime.Now };
                Entry newEntry = context.Set<Entry>().Add(entryToSave).Entity;
                this.context.SaveChanges();
                return newEntry;
            }
            else
            {
                var asd =(latestPurchase.PurchaseDate.AddDays(pass.Days) - latestPurchase.PurchaseDate).TotalSeconds;
                if ((DateTime.Now - latestPurchase.PurchaseDate).TotalSeconds > (latestPurchase.PurchaseDate.AddDays(pass.Days) - latestPurchase.PurchaseDate).TotalSeconds)
                    throw new Exception("A felhasználó legutóbb vásárolt bérlete nem érvényes már.");
                List<Entry> entriesByAccountId = GetEntriesByAccountId(token, accountId, latestPurchase.PurchaseDate.ToString(),pass.MaxEntries);
                if (entriesByAccountId.Count == pass.MaxEntries) throw new Exception("Ezzel a bérlettel nem lehet többször belépni.");
                Entry entryToSave = new Entry() { AccountId = account.Id, EntryDate = DateTime.Now };
                Entry newEntry = context.Set<Entry>().Add(entryToSave).Entity;
                this.context.SaveChanges();
                return newEntry;

            }
        }

        private List<Entry> GetEntriesByAccountId(int accountId, string fromDate = "2025.01.01")
        {
            DateTime from;
            if (!DateTime.TryParse(fromDate,CultureInfo.InvariantCulture,out from))
                throw new Exception("Nem magyar formátumú a dátum.");
            return context.Set<Entry>().Where(e=>e.AccountId==accountId && e.EntryDate>from).OrderByDescending(e=>e.EntryDate).Take(10).ToList();
        }

        public List<Entry> GetEntriesByAccountId(string token, int accountId, string fromDate = "2025.01.01 00:00:00", int limit = 10, int offset = 0, string orderDirection = "desc")
        {
            Account? accountFromToken = accountRepository.GetAccountById(tokenGenerator.GetAccountIdFromToken(token));
            if (accountFromToken == null)
                throw new Exception("Érvénytelen token.");
            Account? account = accountRepository.GetAccountById(accountId);
            if (accountFromToken.Role == "admin" && account == null)
                return null;
            else if (accountFromToken.Role != "admin" && account == null)
                throw new UnauthorizedAccessException();
            if (accountId != tokenGenerator.GetAccountIdFromToken(token) && accountFromToken.Role != "admin" && accountFromToken.Role != "employee")
                throw new UnauthorizedAccessException();
            DateTime from;
            string[] dateTimeFormats = {
                "yyyy.MM.dd HH:mm:ss", "yyyy-MM-dd HH:mm:ss", "MM/dd/yyyy HH:mm:ss", "dd/MM/yyyy HH:mm:ss",
                "yyyy/MM/dd HH:mm:ss", "dd.MM.yyyy HH:mm:ss", "yyyy.MM.dd", "yyyy-MM-dd", "MM/dd/yyyy", "dd/MM/yyyy",
                "yyyy/MM/dd", "dd.MM.yyyy", "G", "F", "O"
            };

            if (!DateTime.TryParseExact(fromDate, dateTimeFormats, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal, out from))
                throw new Exception($"Érvénytelen dátum-idő formátum: {fromDate}");
            if (orderDirection == "asc")
                return context.Set<Entry>().Where(e => e.AccountId == accountId && e.EntryDate > from).OrderBy(e => e.EntryDate).Skip(offset).Take(limit).ToList();
            return context.Set<Entry>().Where(e => e.AccountId == accountId && e.EntryDate > from).OrderByDescending(e => e.EntryDate).Skip(offset).Take(limit).ToList();
        }

        public List<EntryResponse> GetAllEntries(string fromDate = "2025.01.01 00:00:00", int limit=10, int offset=0, string orderField="date",string orderDirection = "desc")
        {
            DateTime from;
            string[] dateTimeFormats = {
                "yyyy.MM.dd HH:mm:ss", "yyyy-MM-dd HH:mm:ss", "MM/dd/yyyy HH:mm:ss", "dd/MM/yyyy HH:mm:ss",
                "yyyy/MM/dd HH:mm:ss", "dd.MM.yyyy HH:mm:ss", "yyyy.MM.dd", "yyyy-MM-dd", "MM/dd/yyyy", "dd/MM/yyyy",
                "yyyy/MM/dd", "dd.MM.yyyy", "G", "F", "O"
            };

            if (!DateTime.TryParseExact(fromDate, dateTimeFormats, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal, out from))
                throw new Exception($"Érvénytelen dátum-idő formátum: {fromDate}");
            List<EntryResponse> result = new List<EntryResponse>();
                var entries = context.Set<Entry>().Where(e => e.EntryDate > from).ToList();
                foreach (Entry e in entries)
                {
                    result.Add(new EntryResponse(accountRepository.GetAccountByIdEvenDeletedOne(e.AccountId)!, e));
                }
            if (orderField=="name")
            {
                if (orderDirection=="asc")
                {
                    return result.OrderBy(e => e.Name).Skip(offset).Take(limit).ToList();
                }
                return result.OrderByDescending(e => e.Name).Skip(offset).Take(limit).ToList();

            }
            if (orderDirection == "asc")
            {
                return result.OrderBy(e => e.EntryDate).Skip(offset).Take(limit).ToList();
            }
            return result.OrderByDescending(e => e.EntryDate).Skip(offset).Take(limit).ToList();

        }

        public int GetEntriesCountByUserId(string token, int accountId)
        {
            Account? accountFromToken = accountRepository.GetAccountById(tokenGenerator.GetAccountIdFromToken(token));
            if (accountFromToken == null)
                throw new Exception("Érvénytelen token.");
            Account? account = accountRepository.GetAccountById(accountId);
            if (accountFromToken.Role == "admin" && account == null)
                return 0;
            else if (accountFromToken.Role != "admin" && account == null)
                throw new Exception("Nem lehet más belépéseit lekérni.");
            if (accountId != tokenGenerator.GetAccountIdFromToken(token) && accountFromToken.Role != "admin")
                throw new Exception("Nem lehet más belépéseit lekérni.");

            return context.Set<Entry>().Where(e => e.AccountId == accountId).Count();
        }

        public int GetAllEntriesCount()
        {
            return context.Set<Entry>().Count();
        }
    }
}
