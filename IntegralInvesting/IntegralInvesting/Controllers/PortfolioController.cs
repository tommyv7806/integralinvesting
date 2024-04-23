using IntegralInvesting.Areas.Identity.Data;
using IntegralInvesting.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IntegralInvesting.Controllers
{
    public class PortfolioController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7226/api");
        private readonly HttpClient _httpClient;
        private readonly UserManager<IntegralInvestingUser> _userManager;

        public PortfolioController(UserManager<IntegralInvestingUser> userManager)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
            _userManager = userManager;
        }

        // Display the information for the user's Portfolio
        public ActionResult Index()
        {
            ValidateUserIsLoggedIn();
            var currentUserId = _userManager.GetUserId(this.User);
            var userPortfolio = GetPortfolioForCurrentUser(currentUserId);

            return View(userPortfolio.PortfolioStocks);
        }

        private void ValidateUserIsLoggedIn()
        {
            // If the user is not logged in, return them to the Login page
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("/");
        }

        private PortfolioViewModel GetPortfolioForCurrentUser(string currentUserId)
        {
            PortfolioViewModel portfolio = new PortfolioViewModel();
            var response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Portfolio/GetUserPortfolio/" + currentUserId).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                portfolio = JsonConvert.DeserializeObject<List<PortfolioViewModel>>(data).Single();
            }

            return portfolio;
        }
    }
}
