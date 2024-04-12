using IntegralInvesting.Areas.Identity.Data;
using IntegralInvesting.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            // If the user is not logged in, return them to the Login page
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("/");

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
    }
}
