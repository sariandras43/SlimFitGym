using SlimFitGym.EFData.Interfaces;
using SlimFitGym.Models.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.EFData.Repositories
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly SlimFitGymContext context;

        public StatisticsRepository(SlimFitGymContext context)
        {
            this.context = context;
        }

        public List<dynamic> PurchasesAndIncomePerMonth(int year)
        {
            if (year < 2025)
                throw new Exception("Csak 2024 utáni évszám adható meg.");
            List<dynamic> result = new List<dynamic>();
            List<Purchase> purchases = this.context.Set<Purchase>().Where(p=>p.PurchaseDate.Year==year).ToList();
            for (int month = 1; month < 13; month++)
            {
                List<Purchase> purchasesPerMonth = purchases.Where(p => p.PurchaseDate.Month == month).ToList();
                decimal sum = 0;
                foreach (Purchase purchase in purchasesPerMonth)
                {
                    sum+= this.context.Set<Pass>().Single(p => p.Id == purchase.PassId).Price;
                }
                result.Add(new { month= month, count = purchasesPerMonth.Count(), income = sum});
            }
            return result;
        }

        public List<dynamic> EntriesPerMonth(int year)
        {
            if (year < 2025)
                throw new Exception("Csak 2024 utáni évszám adható meg.");
            List<dynamic> result = new List<dynamic>();
            List<Entry> entries = this.context.Set<Entry>().Where(e => e.EntryDate.Year == year).ToList();
            for (int month = 1; month < 13; month++)
            {
                List<Entry> entriesPerMonth = entries.Where(e => e.EntryDate.Month == month).ToList();
                result.Add(new { month = month, count = entriesPerMonth.Count()});
            }
            return result;
        }
    }
}
