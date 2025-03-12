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
    public interface IAccountRepository
    {
        AccountResponse? Login(LoginRequest login);
        AccountResponse? Register(RegistrationRequest registration);
        AccountResponse? UpdateAccountPublic(string token, int id, ModifyAccountRequest request);
        AccountResponse? DeleteAccount(string token, int id);
        Account? GetAccountById(int id);
        AccountResponse? BecomeATrainer(int id);
        AccountResponse? GetLoggedInAccountFromToken(string token);
    }
}
