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
    public interface IPurchasesRepository
    {
        List<PurchaseResponse> GetAllPurchases();
        List<Purchase>? GetPurchasesByAccountId(string token, int accountId);
        Purchase? GetLatestPurchaseByAccountId(string token, int accountId);
        Purchase? GetLatestPurchaseByAccountId(int accountId);
        PurchaseResponse? GetPurchaseById(int id);
        PurchaseResponse? NewPurchase(string token, PurchaseRequest purchase);
    }
}
