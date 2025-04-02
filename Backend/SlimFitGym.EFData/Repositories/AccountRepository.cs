using BCrypt.Net;
using SlimFitGym.EFData.Interfaces;
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
    public class AccountRepository:IAccountRepository
    {
        readonly SlimFitGymContext context;
        readonly TokenGenerator tokenGenerator;
        readonly IImagesRepository imagesRepository;

        public AccountRepository(SlimFitGymContext context, TokenGenerator tokenGenerator, IImagesRepository imagesRepository)
        {
            this.context = context;
            this.tokenGenerator = tokenGenerator;
            this.imagesRepository = imagesRepository;
        }

        public AccountResponse? Login(LoginRequest login)
        {
            Account? a = context.Set<Account>().SingleOrDefault(a => a.Email == login.Email && a.isActive);

            if (a==null || !BCrypt.Net.BCrypt.EnhancedVerify(login.Password, a.Password))
                throw new Exception("Helytelen email cím vagy jelszó.");
            DateTime validTo = DateTime.Now.AddDays(1);
            if (login.RememberMe)
                validTo = DateTime.Now.AddDays(364);

            string? url = imagesRepository.GetImageUrlByAccountId(a.Id);
            if (url!=null)
            {
                return new AccountResponse(a,tokenGenerator.GenerateToken(a.Id,a.Email,login.RememberMe,a.Role),validTo,url);

            }
            return new AccountResponse(a, tokenGenerator.GenerateToken(a.Id, a.Email, login.RememberMe, a.Role), validTo);
        }

        public AccountResponse? Register(RegistrationRequest registration)
        {
            if (string.IsNullOrWhiteSpace(registration.Name))
                throw new Exception("Név kitöltése kötelező.");
            if (registration.Name.Length > 100)
                throw new Exception("Túl hosszú név.");
            if (string.IsNullOrEmpty(registration.Password))
                throw new Exception("Jelszót kötelező megadni.");
            if (!Regex.IsMatch(registration.Password, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[#?!@$%^&*_-]).{8,}$"))
                throw new Exception("Nem megfelelő a jelszó: 8 karakter minimum, szám, kis- és nagybetűnek és egy speciális karakter kombinációjának kell lennie.");
            Regex emailRegex = new Regex(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])");
            Match email = emailRegex.Match(registration.Email);
            if (!email.Success)
                throw new Exception("Érvénytelen formátumú email cím.");
            Regex phoneRegex = new Regex(@"\+(9[976]\d|8[987530]\d|6[987]\d|5[90]\d|42\d|3[875]\d|2[98654321]\d|9[8543210]|8[6421]|6[6543210]|5[87654321]|4[987654310]|3[9643210]|2[70]|7|1)\d{1,14}$");
            Match phone = phoneRegex.Match(registration.Phone.Replace(" ", "").Replace("-", ""));
            if (!phone.Success)
                throw new Exception("Érvénytelen formátumú telefonszám.");
            if (context.Set<Account>().Any(a => a.Email == registration.Email))
                throw new Exception("Ezzel az email címmel már regisztráltak.");
            if (context.Set<Account>().Any(a => a.Phone == registration.Phone))
                throw new Exception("Ez a telefonszám már használatban van.");
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

            DateTime validTo = DateTime.Now.AddDays(1);
            if (registration.RememberMe)
                validTo = DateTime.Now.AddDays(364);
            Account savedAccount = this.context.Set<Account>().Add(newAccount).Entity;
            this.context.SaveChanges();
            return new AccountResponse(savedAccount, tokenGenerator.GenerateToken(savedAccount.Id,savedAccount.Email, false, savedAccount.Role),validTo);

        }


        public AccountResponse? UpdateAccountPublic(string token,int id, ModifyAccountRequest request)
        {
            if (tokenGenerator.GetAccountIdFromToken(token) != id)
                throw new UnauthorizedAccessException();
            if (request == null)
                throw new Exception("Érvénytelen kérés.");
            if (request.Id != id)
                throw new Exception("Érvénytelen azonosító.");
            Account? account = context.Set<Account>().SingleOrDefault(a => a.Id == request.Id && a.isActive);
            if (account == null)
                return null;
            if (account.Id != request.Id)
                throw new Exception("Érvénytelen azonosító.");
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                if(request.Name.Length > 4 && request.Name.Length < 100)
                    account.Name = request.Name;
                else
                    throw new Exception("Név minimum 4, maximum 100 karakter hosszú lehet.");

            }

            if (!string.IsNullOrWhiteSpace(request.Phone))
            {
                if (context.Set<Account>().Any(a => a.Phone == request.Phone))
                    throw new Exception("Ez a telefonszám már használatban van.");
            Regex phoneRegex = new Regex(@"\+(9[976]\d|8[987530]\d|6[987]\d|5[90]\d|42\d|3[875]\d|2[98654321]\d|9[8543210]|8[6421]|6[6543210]|5[87654321]|4[987654310]|3[9643210]|2[70]|7|1)\d{1,14}$");
            Match phone = phoneRegex.Match(request.Phone.Replace(" ", "").Replace("-", ""));
            if (!phone.Success)
                throw new Exception("Érvénytelen formátumú telefonszám.");
            account.Phone = request.Phone;

            }

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                Regex emailRegex = new Regex(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])");
                Match email = emailRegex.Match(request.Email);
                if (!email.Success)
                    throw new Exception("Érvénytelen formátumú email cím.");
                if (request.Email != account.Email && context.Set<Account>().Any(a => a.Email == request.Email))
                    throw new Exception("Ezzel az email címmel már regisztráltak.");
                account.Email = request.Email;

            }


            if (!string.IsNullOrWhiteSpace(request.NewPassword))
            {
                Regex newPasswordRegex = new Regex(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[#?!@$%^&*_-]).{8,}$");
                if (!string.IsNullOrWhiteSpace(request.NewPassword))
                {
                    Match newPassword = newPasswordRegex.Match(request.NewPassword);
                    if (!newPassword.Success)
                        throw new Exception("Nem megfelelő az új jelszó: 8 karakter minimum, szám, kis- és nagybetűnek és egy speciális karakter kombinációjának kell lennie.");
                }    
                if (!BCrypt.Net.BCrypt.EnhancedVerify(request.NewPassword, account.Password))
                {
                    string hash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.NewPassword, 10);
                    account.Password = hash;
                }

            }

            if (request.Image!=null)
            {
                Image? image;
                imagesRepository.DeleteImageByAccountId(id);
                if (request.Image!="")
                    image = imagesRepository.UploadImageToAccount(request.Image, id);

                this.context.Entry(account).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                this.context.SaveChanges();
                return new AccountResponse(account) { ImageUrl = imagesRepository.GetImageUrlByAccountId(id) };
            }
            this.context.Entry(account).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            this.context.SaveChanges();
            string url = imagesRepository.GetImageUrlByAccountId(id);

            return new AccountResponse(account, url);
        }

        public AccountResponse? DeleteAccount(string token, int id)
        {
            var accountWhichDeletes = this.context.Set<Account>().SingleOrDefault(a => a.Id == tokenGenerator.GetAccountIdFromToken(token) && a.isActive);
            if (tokenGenerator.GetAccountIdFromToken(token) != id && accountWhichDeletes.Role != "admin")
                throw new UnauthorizedAccessException();
            if (accountWhichDeletes == null)
                throw new Exception("A felhasználó aki törölne, nem létezik");
            if (id <= 0)
                throw new Exception("Érvénytelen azonosító.");
            var accountToDelete = this.context.Set<Account>().SingleOrDefault(a => a.Id == id&&a.isActive);
            if (accountToDelete == null) return null;
            if (this.context.Set<Account>().Where(a=>a.Role=="admin" && a.isActive && a.Id==id).Count()==1)
                throw new Exception("Utolsó adminisztrátor fiók nem törölhető.");
            if (this.context.Set<Account>().Where(a => a.Role == "employee" && a.isActive && a.Id == id).Count() == 1)
                throw new Exception("Utolsó dolgozó fiók nem törölhető.");
            accountToDelete.isActive = false;
            this.context.Entry(accountToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            //TODO cascading delete at reservations and trainings, images
            this.context.SaveChanges();
            return new AccountResponse(accountToDelete);

        }

        public AccountResponse? GetLoggedInAccountFromToken(string token)
        {
            var account = this.context.Set<Account>().SingleOrDefault(a => a.Id == tokenGenerator.GetAccountIdFromToken(token));
            if (account == null) return null;
            return new AccountResponse(account,imagesRepository.GetImageUrlByAccountId(account.Id));
        }

        public Account? GetAccountById(int id)
        {
            if (id <= 0)
                throw new Exception("Nincs ilyen felhasználó");
            return context.Set<Account>().SingleOrDefault(a => a.Id == id&&a.isActive);
        }

        public Account? GetAccountByIdEvenDeletedOne(int id)
        {
            if (id <= 0)
                throw new Exception("Nincs ilyen felhasználó");
            return context.Set<Account>().SingleOrDefault(a => a.Id == id && a.isActive);
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

        public List<AccountResponse> GetAllAccounts(bool onlyActive = false)
        {
            if (onlyActive)
                return this.context.Set<Account>().Where(a=>a.isActive).Select(a=>new AccountResponse(a,imagesRepository.GetImageUrlByAccountId(a.Id))).ToList();    
            return this.context.Set<Account>().Select(a=>new AccountResponse(a,imagesRepository.GetImageUrlByAccountId(a.Id))).ToList();
        }

        public List<AccountResponse> GetTrainers(bool onlyActive = true)
        {
            if (onlyActive)
                return this.context.Set<Account>().Where(a => a.isActive && a.Role=="trainer").Select(a => new AccountResponse(a, imagesRepository.GetImageUrlByAccountId(a.Id))).ToList();
            return this.context.Set<Account>().Select(a => new AccountResponse(a, imagesRepository.GetImageUrlByAccountId(a.Id))).ToList();
        }
    }
}
