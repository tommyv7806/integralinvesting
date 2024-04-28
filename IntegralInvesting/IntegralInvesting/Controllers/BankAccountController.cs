using IntegralInvesting.Areas.Identity.Data;
using IntegralInvesting.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace IntegralInvesting.Controllers
{
    public class BankAccountController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7226/api");
        private readonly HttpClient _httpClient;
        private readonly UserManager<IntegralInvestingUser> _userManager;

        public BankAccountController(UserManager<IntegralInvestingUser> userManager)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
            _userManager = userManager;
        }

        // Runs when the user clicks the Bank Accounts link at the top of the page
        [HttpGet]   
        public IActionResult Index()
        {
            ValidateUserIsLoggedIn();

            var currentUserId = _userManager.GetUserId(this.User);
            var currentUserBankAccounts = GetBankAccountsForCurrentUser(currentUserId);
            var currentUserFunds = GetFundsForCurrentUser(currentUserId);

            ViewData["CurrentUserFunds"] = currentUserFunds;
            return View(currentUserBankAccounts);
        }

        // Opens the modal where users can link new bank accounts to their app account
        [HttpGet]
        public IActionResult OpenLinkNewAccountModal()
        {
            return PartialView("LinkNewAccountModalPartial");
        }

        // When the user saves the newly linked account
        [HttpPost]
        public IActionResult SaveLinkedAccount(BankAccountViewModel bankAccount)
        {
            ValidateUserIsLoggedIn();

            var currentUserId = _userManager.GetUserId(this.User);
            bankAccount.UserId = currentUserId;

            try
            {
                string data = JsonConvert.SerializeObject(bankAccount);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/bankAccount/Post", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Bank Account successfully linked";
                    return RedirectToAction("Index", "BankAccount");
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                return View();
            }

            return View();            
        }

        [HttpGet]
        public IActionResult OpenWithdrawFundsModal()
        {
            ValidateUserIsLoggedIn();

            var currentUserId = _userManager.GetUserId(this.User);
            var accountNames = GetAccountNamesForCurrentUser(currentUserId);
            var currentUserFund = GetUserFundForCurrentUser(currentUserId);

            ViewData["BankAccountNames"] = accountNames;
            return PartialView("WithdrawFundsModalPartial", currentUserFund);
        }

        // When the user wants to withdraw funds from their linked bank account
        [HttpGet]
        public IActionResult WithdrawFunds()
        {
            ValidateUserIsLoggedIn();

            var currentUserId = _userManager.GetUserId(this.User);
            var accountNames = GetAccountNamesForCurrentUser(currentUserId);
            var currentUserFund = GetUserFundForCurrentUser(currentUserId);

            ViewData["BankAccountNames"] = accountNames;
            return View(currentUserFund);
        }

        [HttpPost]
        public IActionResult WithdrawTransaction(UserFundViewModel userFund)
        {
            ValidateUserIsLoggedIn();

            if (userFund.CurrentTransferAmount != null && userFund.CurrentTransferAmount >= 0)
                userFund.CurrentFunds += (decimal)userFund.CurrentTransferAmount;

            try
            {
                string data = JsonConvert.SerializeObject(userFund);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                var stringContent = content.ReadAsStringAsync().Result;
                HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "/UserFund/Put", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = $"Funds successfully withdrawn from {userFund.CurrentTransferAccount} bank account";
                    userFund.CurrentTransferAmount = null;
                    userFund.CurrentTransferAccount = null;

                    return RedirectToAction("Index", "BankAccount");
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                return View();
            }

            return View();
        }

        [HttpGet]
        public IActionResult OpenDepositFundsModal()
        {
            ValidateUserIsLoggedIn();

            var currentUserId = _userManager.GetUserId(this.User);
            var accountNames = GetAccountNamesForCurrentUser(currentUserId);
            var currentUserFund = GetUserFundForCurrentUser(currentUserId);
            
            ViewData["BankAccountNames"] = accountNames;

            return PartialView("DepositFundsModalPartial", currentUserFund);
        }

        [HttpGet]
        public IActionResult DepositFunds()
        {
            ValidateUserIsLoggedIn();

            var currentUserId = _userManager.GetUserId(this.User);
            var accountNames = GetAccountNamesForCurrentUser(currentUserId);
            var currentUserFund = GetUserFundForCurrentUser(currentUserId);

            ViewData["BankAccountNames"] = accountNames;
            return View(currentUserFund);
        }

        [HttpPost]
        public IActionResult DepositTransaction(UserFundViewModel userFund)
        {
            ValidateUserIsLoggedIn();

            if (userFund.CurrentTransferAmount != null && userFund.CurrentTransferAmount <= userFund.CurrentFunds)
                userFund.CurrentFunds -= (decimal)userFund.CurrentTransferAmount;

            try
            {
                string data = JsonConvert.SerializeObject(userFund);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                var stringContent = content.ReadAsStringAsync().Result;
                HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "/UserFund/Put", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = $"Funds successfully deposited into {userFund.CurrentTransferAccount} bank account";
                    userFund.CurrentTransferAmount = null;
                    userFund.CurrentTransferAccount = null;

                    return RedirectToAction("Index", "BankAccount");
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                return View();
            }

            return View();
        }

        [HttpGet]
        public IActionResult RemoveBankAccount(int id)
        {
            DeleteBankAccount(id);

            return RedirectToAction("Index");
        }

        private void ValidateUserIsLoggedIn()
        {
            // If the user is not logged in, return them to the Login page
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("/");
        }

        private List<BankAccountViewModel> GetBankAccountsForCurrentUser(string currentUserId)
        {
            List<BankAccountViewModel> bankAccountList = new List<BankAccountViewModel>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/BankAccount/GetUserAccounts/" + currentUserId).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                bankAccountList = JsonConvert.DeserializeObject<List<BankAccountViewModel>>(data);
            }

            return bankAccountList;
        }

        private decimal GetFundsForCurrentUser(string currentUserId)
        {
            UserFundViewModel userFund = new UserFundViewModel();
            var response = _httpClient.GetAsync(_httpClient.BaseAddress + "/UserFund/GetUserFunds/" + currentUserId).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                userFund = JsonConvert.DeserializeObject<List<UserFundViewModel>>(data).Single();
            }

            return userFund.CurrentFunds;
        }

        private UserFundViewModel GetUserFundForCurrentUser(string currentUserId)
        {
            UserFundViewModel userFund = new UserFundViewModel();
            var response = _httpClient.GetAsync(_httpClient.BaseAddress + "/UserFund/GetUserFunds/" + currentUserId).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                userFund = JsonConvert.DeserializeObject<List<UserFundViewModel>>(data).Single();
            }

            return userFund;
        }

        private List<string> GetAccountNamesForCurrentUser(string currentUserId)
        {
            var currentUserBankAccounts = GetBankAccountsForCurrentUser(currentUserId);

            var accountNames = new List<string>();
            foreach (var account in currentUserBankAccounts)
            {
                accountNames.Add(account.AccountName);
            }

            return accountNames;
        }

        private void DeleteBankAccount(int id)
        {
            try
            {
                var response = _httpClient.DeleteAsync(_httpClient.BaseAddress + "/BankAccount/Delete/" + id).Result;
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }
        }
    }
}