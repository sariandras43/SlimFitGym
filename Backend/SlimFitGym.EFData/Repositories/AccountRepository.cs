using BCrypt.Net;
using SlimFitGym.Models.Models;
using SlimFitGym.Models.Requests;
using SlimFitGym.Models.Responses;
using SlimFitGymBackend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SlimFitGym.EFData.Repositories
{
    public class AccountRepository
    {
        readonly SlimFitGymContext context;
        readonly TokenGenerator tokenGenerator;

        public AccountRepository(SlimFitGymContext context, TokenGenerator tokenGenerator)
        {
            this.context = context;
            this.tokenGenerator = tokenGenerator;
        }

        public AccountResponse? Login(LoginRequest login)
        {
            Account? a = context.Set<Account>().SingleOrDefault(a => a.Email == login.Email);
            //string hash = BCrypt.Net.BCrypt.EnhancedHashPassword(login.Password, 10);
#if DEBUG
            if (a==null || a.Password!=login.Password)
                throw new Exception("Helytelen email cím vagy jelszó.");
            //else if (!BCrypt.Net.BCrypt.EnhancedVerify(login.Password, a.Password))
            //    throw new Exception("Helytelen email cím vagy jelszó.");
#else

            if (!BCrypt.Net.BCrypt.EnhancedVerify(login.Password, a.Password))
                throw new Exception("Helytelen email cím vagy jelszó.");


#endif
            return new AccountResponse(a,tokenGenerator.GenerateToken(a.Email,false,a.Role));

        }

        public AccountResponse? Register(RegistrationRequest registration)
        {
            if (registration.Name.Length > 100)
                throw new Exception("Túl hosszú név.");
            if (registration.Password.Length < 8)
                throw new Exception("Túl rövid jelszó.");
            Regex emailRegex = new Regex(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])");
            Match email = emailRegex.Match(registration.Email);
            if (!email.Success)
                throw new Exception("Érvénytelen formátumú email cím.");

            bool isMadeUpOfDigits = true;
            for (int i = 1; i < registration.Phone.Length; i++)
            {
                if (!char.IsDigit(registration.Phone[i]))
                    isMadeUpOfDigits = false;
            }
            if (!registration.Phone.StartsWith('+') || registration.Phone.Length > 16 || !isMadeUpOfDigits || registration.Phone.Length < 7)
                throw new Exception("Érvénytelen formátumú telefonszám.");
            Account? account = context.Set<Account>().SingleOrDefault(a => a.Email == registration.Email);
            if (account != null)
                throw new Exception("Ezzel az email címmel már regisztráltak");

            string hash = BCrypt.Net.BCrypt.EnhancedHashPassword(registration.Password, 10);
            Account newAccount = new Account()
            {
                Email = registration.Email,
                isActive = true,
                Name = registration.Name,
                Password = hash,
                Phone = registration.Phone.Replace(" ", "").Replace("-",""),
                Role = "user"
            };

            Account savedAccount = this.context.Set<Account>().Add(newAccount).Entity;
            this.context.SaveChanges();
            return new AccountResponse(savedAccount, tokenGenerator.GenerateToken(savedAccount.Email, false, savedAccount.Role));

        }

        private AccountResponse? UpdateAccount(int id, ModifyAccountRequest request)
        {
            if (request.Id != id)
                throw new Exception("Érvénytelen azonosító.");
            if (request == null)
                throw new Exception("Érvénytelen kérés.");
            if (!string.IsNullOrEmpty(request.Name) && request.Name.Length > 100)
                throw new Exception("Túl hosszú név.");
            Regex emailRegex = new Regex(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])");
            Match email = emailRegex.Match(request.Email);
            if (!email.Success)
                throw new Exception("Érvénytelen formátumú email cím.");
            bool isMadeUpOfDigits = true;
            for (int i = 1; i < request.Phone.Length; i++)
            {
                if (!char.IsDigit(request.Phone[i]))
                    isMadeUpOfDigits = false;
            }
            if (!string.IsNullOrEmpty(request.Phone) && !request.Phone.StartsWith('+') || request.Phone.Length > 16 || !isMadeUpOfDigits || request.Phone.Length < 7)
                throw new Exception("Érvénytelen formátumú telefonszám.");
            Account? account = context.Set<Account>().SingleOrDefault(a => a.Email == request.Email);
            if (account == null)
                return null;
            if (account.Id != request.Id)
                throw new Exception("Érvénytelen azonosító.");
            if (request.Email != account.Email)
                if (context.Set<Account>().Any(a => a.Email == request.Email))
                    throw new Exception("Ezzel az email címmel már regisztráltak");
            account.Email = request.Email;

            if (!string.IsNullOrEmpty(request.Password) && request.Password.Length < 8)
                throw new Exception("Túl rövid jelszó.");

            if (request.Phone != account.Phone)
                if (context.Set<Account>().Any(a => a.Phone == request.Phone))
                    throw new Exception("Ez a telefonszám már használatban van.");
            account.Phone = request.Phone;

            if (!BCrypt.Net.BCrypt.EnhancedVerify(request.Password, account.Password))
            {
                string hash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password, 10);
                account.Password = hash;
            }


            account.Name = request.Name;
            account.Role = request.Role;

            this.context.Entry(account).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            this.context.SaveChanges();
            return new AccountResponse(account);
        }

        public AccountResponse? UpdateAccountPublic(int id, ModifyAccountRequest request)
        {
            if (request.Id != id)
                throw new Exception("Érvénytelen azonosító.");
            if (request == null)
                throw new Exception("Érvénytelen kérés.");
            Account? account = context.Set<Account>().SingleOrDefault(a => a.Id == request.Id);
            if (account == null)
                return null;
            if (account.Id != request.Id)
                throw new Exception("Érvénytelen azonosító.");
            if (!string.IsNullOrEmpty(request.Name))
            {
                if (request.Name.Length > 100)
                    throw new Exception("Túl hosszú név.");
                account.Name = request.Name;

            }
            if (!string.IsNullOrEmpty(request.Phone))
            {
                bool isMadeUpOfDigits = true;
                for (int i = 1; i < request.Phone.Length; i++)
                {
                    if (!char.IsDigit(request.Phone[i]))
                        isMadeUpOfDigits = false;
                }
                if (request.Phone != account.Phone)
                    if (context.Set<Account>().Any(a => a.Phone == request.Phone))
                        throw new Exception("Ez a telefonszám már használatban van.");
                if (!string.IsNullOrEmpty(request.Phone) && !request.Phone.StartsWith('+') || request.Phone.Length > 16 || !isMadeUpOfDigits || request.Phone.Length < 7)
                    throw new Exception("Érvénytelen formátumú telefonszám.");
                account.Phone = request.Phone;
            }

            if (!string.IsNullOrEmpty(request.Email) && request.Email != account.Email)
            {
                Regex emailRegex = new Regex(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])");
                Match email = emailRegex.Match(request.Email);
                if (!email.Success)
                    throw new Exception("Érvénytelen formátumú email cím.");
                if (context.Set<Account>().Any(a => a.Email == request.Email))
                    throw new Exception("Ezzel az email címmel már regisztráltak");
                account.Email = request.Email;
            }

            if (!string.IsNullOrEmpty(request.Password) && request.Password.Length < 8)
                throw new Exception("Túl rövid jelszó.");


            if (!BCrypt.Net.BCrypt.EnhancedVerify(request.Password, account.Password))
            {
                string hash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password, 10);
                account.Password = hash;
            }



            this.context.Entry(account).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            this.context.SaveChanges();
            return new AccountResponse(account);
        }

        public AccountResponse? DeleteAccount(int id)
        {
            if (id <= 0)
                throw new Exception("Érvénytelen azonosító.");
            var accountToDelete = this.context.Set<Account>().SingleOrDefault(a => a.Id == id);
            if (accountToDelete == null)
                return null;

            accountToDelete.isActive = false;
            this.context.Entry(accountToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            this.context.SaveChanges();
            return new AccountResponse(accountToDelete);

        }

        public Account? GetAccountById(int id)
        {
            if (id <= 0)
                throw new Exception("Nincs ilyen felhasználó");
            return context.Set<Account>().SingleOrDefault(a => a.Id == id&&a.isActive);
        }

        public AccountResponse? BecomeATrainer(int id)
        {
            if (id <= 0)
                throw new Exception("Nincs ilyen felhasználó");
            Account? account = context.Set<Account>().SingleOrDefault(a => a.Id == id);
            if (account == null)
                return null;
            account.Role = "trainer";
            this.context.Entry(account).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            this.context.SaveChanges();
            return new AccountResponse(account);
        }
    }
}
