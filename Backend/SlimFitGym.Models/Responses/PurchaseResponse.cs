using SlimFitGym.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Models.Responses
{
    public class PurchaseResponse
    {
        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int PassId { get; set; }
        public int AccountId { get; set; }

        public PurchaseResponse(Purchase p)
        {
            Id = p.Id;
            PurchaseDate = p.PurchaseDate.ToUniversalTime();
            PassId = p.PassId;
            AccountId = p.AccountId;
        }
        public PurchaseResponse()
        {
            
        }
    }
}
