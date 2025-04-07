using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.EFData.Interfaces
{
    public interface IStatisticsRepository
    {
        public List<dynamic> PurchasesAndIncomePerMonth(int year);
    }
}
