﻿using IntegralInvesting.Areas.Identity.Data;
using IntegralInvesting.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Sockets;
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

        [HttpGet]
        public IActionResult Index()
        {
            ValidateUserIsLoggedIn();

            var currentUserId = _userManager.GetUserId(this.User);

            List<BankAccountViewModel> bankAccountList = new List<BankAccountViewModel>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/BankAccount/GetUserAccounts/" + currentUserId).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                bankAccountList = JsonConvert.DeserializeObject<List<BankAccountViewModel>>(data);
            }

            ViewData["UserId"] = currentUserId;
            return View(bankAccountList);
        }

        [HttpGet]
        public IActionResult LinkNewAccount()
        {
            ValidateUserIsLoggedIn();

            return View();
        }

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

        private void ValidateUserIsLoggedIn()
        {
            // If the user is not logged in, return them to the Login page
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("/");
        }
    }
}