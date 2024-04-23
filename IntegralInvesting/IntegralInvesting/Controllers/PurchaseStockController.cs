using IntegralInvesting.Areas.Identity.Data;
using IntegralInvesting.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceStack;
using System.Text;

namespace IntegralInvesting.Controllers
{
    public class PurchaseStockController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7226/api");
        private readonly HttpClient _httpClient;
        private readonly UserManager<IntegralInvestingUser> _userManager;
        private readonly IConfiguration _config;
        private readonly string? _apiKey;

        public PurchaseStockController(UserManager<IntegralInvestingUser> userManager, IConfiguration config)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
            _userManager = userManager;
            _config = config;

            _apiKey = _config.GetValue<string>("AlphaVantageSettings:ApiKey:Key");
        }

        // Displays the Purchase page without any search results - Only displays the search bar
        [HttpGet]
        public IActionResult Index(string searchString)
        {
            return View();
        }

        // After the user clicks the search button, display the initial search results on left side of screen
        public IActionResult InitialSearch(string searchString)
        {
            ViewData["SearchQuery"] = searchString;

            if (searchString != null)
            {
                var stockApiResponse = $"https://www.alphavantage.co/query?function=SYMBOL_SEARCH&keywords={searchString}&apikey={_apiKey}&datatype=csv"
                    .GetStringFromUrl();

                if (stockApiResponse.Contains("Invalid API call"))
                    return PartialView("InitialSearchResultPartial", new List<StockSearchViewModel>());

                var results = stockApiResponse.FromCsv<List<StockSearchViewModel>>().ToList();
                var filteredResults = results.Where(r => r.Symbol.Length < 5).ToList();

                return PartialView("InitialSearchResultPartial", filteredResults);
            }

            return PartialView("InitialSearchResultPartial", new List<StockSearchViewModel>());
        }

        // When the user clicks on one of the search results, display the details for that stock
        public IActionResult DetailsSearch(string symbol, string stockName)
        {
            if (symbol != null)
            {
                var stockApiResponse = $"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={symbol}&apikey={_apiKey}&interval=60min&datatype=csv"
                    .GetStringFromUrl();

                if (stockApiResponse.Contains("Invalid API call"))
                {
                    ViewData["ErrorMessage"] = "Please enter a valid stock symbol (e.g., MSFT, AAPL, etc.)";
                    return PartialView("SearchResultDetailsPartial", new List<StockDetailsViewModel>());
                }

                var allPrices = stockApiResponse.FromCsv<List<StockDetailsViewModel>>().ToList();

                ViewData["Symbol"] = symbol;
                ViewData["StockName"] = stockName;
                ViewData["LatestPrice"] = allPrices.First().Close;
                return PartialView("SearchResultDetailsPartial", allPrices);
            }
            
            return PartialView("SearchResultDetailsPartial", new List<StockDetailsViewModel>());
        }

        // Opens the modal where users can enter the number of shares of a particular stock they want to purchase
        [HttpGet]
        public IActionResult OpenPurchaseModal(string stockName, string latestPrice, string symbol)
        {
            var model = new PortfolioStockViewModel();
            model.Name = stockName;
            model.PurchasePrice = decimal.Parse(latestPrice);
            model.Symbol = symbol;

            var currentUserId = _userManager.GetUserId(this.User);
            var userFunds = GetFundsForCurrentUser(currentUserId).CurrentFunds;

            ViewData["UserFunds"] = userFunds;

            return PartialView("PurchaseSharesModalPartial", model);
        }

        [HttpPost]
        public IActionResult PurchaseShares(PortfolioStockViewModel model)
        {
            var currentUserId = _userManager.GetUserId(this.User);

            var userPortfolio = GetPortfolioForCurrentUser(currentUserId);
            model.PortfolioId = userPortfolio.PortfolioId;

            var userFunds = GetFundsForCurrentUser(currentUserId);
            userFunds.CurrentFunds -= model.PurchaseTotal;

            UpdateUserCurrentFunds(userFunds);
            return AddSharesToPortfolio(model);
        }

        private IActionResult AddSharesToPortfolio(PortfolioStockViewModel model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/PortfolioStock/Post", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = $"{model.PurchaseQuantity} shares successfully purchased for {model.Name}";
                    return RedirectToAction("Index", "Portfolio");
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                return PartialView("PurchaseSharesModalPartial", model);
            }

            return PartialView("PurchaseSharesModalPartial", model);
        }

        private void UpdateUserCurrentFunds(UserFundViewModel userFunds)
        {
            try
            {
                string data = JsonConvert.SerializeObject(userFunds);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                var stringContent = content.ReadAsStringAsync().Result;
                HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "/UserFund/Put", content).Result;
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }
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

        private UserFundViewModel GetFundsForCurrentUser(string currentUserId)
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
    }
}
