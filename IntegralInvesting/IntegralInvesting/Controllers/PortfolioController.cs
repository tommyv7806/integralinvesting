using IntegralInvesting.Areas.Identity.Data;
using IntegralInvesting.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Packaging;
using ServiceStack;
using System.Text;

namespace IntegralInvesting.Controllers
{
    public class PortfolioController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7226/api");
        private readonly HttpClient _httpClient;
        private readonly UserManager<IntegralInvestingUser> _userManager;
        private readonly IConfiguration _config;
        private readonly string? _apiKey;

        public PortfolioController(UserManager<IntegralInvestingUser> userManager, IConfiguration config)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
            _userManager = userManager;
            _config = config;
            _apiKey = _config.GetValue<string>("AlphaVantageSettings:ApiKey:Key");
        }

        // Display the information for the user's Portfolio
        public ActionResult Index()
        {
            HttpContext.Session.Clear();
            ValidateUserIsLoggedIn();
            var currentUserId = _userManager.GetUserId(this.User);
            var userPortfolio = GetPortfolioForCurrentUser(currentUserId);

            if (userPortfolio.PortfolioAssets.Count == 0)
                return View(new List<PortfolioAssetViewModel>());

            foreach (var asset in userPortfolio.PortfolioAssets)
            {
                var stockDetails = GetBasicStockDetails(asset.Symbol);
                asset.CurrentPrice = stockDetails.Price;
                asset.NumberOfShares = asset.PortfolioStocks.Sum(ps => ps.PurchaseQuantity);

                PopulateDataForLastWeek(asset);
            }

            return View(userPortfolio.PortfolioAssets);
        }

        private void PopulateDataForLastWeek(PortfolioAssetViewModel asset)
        {
            var stockApiResponse = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={asset.Symbol}&apikey={_apiKey}&datatype=csv"
                    .GetStringFromUrl();

            var results = stockApiResponse.FromCsv<List<StockTimeDetails>>().ToList();

            var lastSevenDaysData = results.Take(7).Reverse().ToList();

            asset.LastSevenDaysData.AddRange(lastSevenDaysData);
        }

        // Opens the modal where users can enter the number of shares of a particular stock they want to sell
        [HttpGet]
        public IActionResult OpenSellModal(string symbol, decimal currentPrice, int numberOfShares)
        {
            ValidateUserIsLoggedIn();
            var currentUserId = _userManager.GetUserId(this.User);
            var userPortfolio = GetPortfolioForCurrentUser(currentUserId);

            var portfolioAsset = GetSelectedPortfolioAsset(symbol, userPortfolio.PortfolioId);
            portfolioAsset.NumberOfShares = numberOfShares;
            portfolioAsset.CurrentPrice = currentPrice;

            return PartialView("SellSharesModalPartial", portfolioAsset);
        }

        [HttpPost]
        public IActionResult SellShares(PortfolioAssetViewModel portfolioAsset)
        {
            ValidateUserIsLoggedIn();
            var currentUserId = _userManager.GetUserId(this.User);

            var userFunds = GetFundsForCurrentUser(currentUserId);
            userFunds.CurrentFunds += portfolioAsset.SaleTotal;
            UpdateUserCurrentFunds(userFunds);


            var portfolioStocks = GetPortfolioStocksForPortfolioAsset(portfolioAsset.PortfolioAssetId);
            portfolioAsset.PortfolioStocks.AddDistinctRange(portfolioStocks);

            var sellQuantity = portfolioAsset.SellQuantity;

            foreach (var stock in portfolioAsset.PortfolioStocks)
            {
                if (sellQuantity > 0)
                {
                    // If num of shares for stock is greater than sell qty, reduce num of shares by sell qty amount
                    if (stock.PurchaseQuantity > sellQuantity)
                    {
                        stock.PurchaseQuantity -= sellQuantity;
                        sellQuantity = 0;

                        UpdatePortfolioStock(stock);
                    }
                    // If num of shares for stock is less than sell qty, reduce num of shares to zero, delete PortfolioStock record, and subtract num of shares from sell qty
                    else if (stock.PurchaseQuantity <= sellQuantity)
                    {
                        sellQuantity -= stock.PurchaseQuantity;

                        // Delete PortfolioStock record
                        DeletePortfolioStock(stock, portfolioAsset.PortfolioId);
                    }
                }
            }

            TempData["SuccessMessage"] = $"Successfully sold {portfolioAsset.SellQuantity} share(s) of {portfolioAsset.Name} stock";
            return RedirectToAction("Index");
        }

        private void DeletePortfolioStock(PortfolioStockViewModel portfolioStock, int portfolioId)
        {
            try
            {
                var response = _httpClient.DeleteAsync(_httpClient.BaseAddress + "/PortfolioStock/Delete/" + portfolioStock.PortfolioStockId).Result;

                if (response.IsSuccessStatusCode)
                {
                    var portfolioAsset = GetSelectedPortfolioAsset(portfolioStock.Symbol, portfolioId);

                    if (portfolioAsset != null && portfolioAsset.PortfolioStocks.Count() == 0)
                        DeletePortfolioAsset(portfolioStock.PortfolioAssetId);
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }
        }

        private void DeletePortfolioAsset(int id)
        {
            var response = _httpClient.DeleteAsync(_httpClient.BaseAddress + "/PortfolioAsset/Delete/" + id).Result;
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

        private void UpdatePortfolioStock(PortfolioStockViewModel portfolioStock)
        {
            try
            {
                string data = JsonConvert.SerializeObject(portfolioStock);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                var stringContent = content.ReadAsStringAsync().Result;
                HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "/PortfolioStock/Put", content).Result;
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }
        }

        private PortfolioAssetViewModel GetSelectedPortfolioAsset(string symbol, int portfolioId)
        {
            PortfolioAssetViewModel portfolioAsset = new PortfolioAssetViewModel();
            var response = _httpClient.GetAsync(_httpClient.BaseAddress + "/PortfolioAsset/GetPortfolioAssetForStockSymbolFromPortfolio/" + symbol + "," + portfolioId).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                portfolioAsset = JsonConvert.DeserializeObject<PortfolioAssetViewModel>(data);
            }
            else
            {
                return null;
            }

            return portfolioAsset;
        }

        private StockDetailsViewModel GetBasicStockDetails(string symbol)
        {
            var stockApiResponse = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={symbol}&apikey={_apiKey}&datatype=csv"
                    .GetStringFromUrl();

            if (stockApiResponse.Contains("Invalid API call"))
            {
                ViewData["ErrorMessage"] = "Please enter a valid stock symbol (e.g., MSFT, AAPL, etc.)";
            }

            return stockApiResponse.FromCsv<StockDetailsViewModel>();
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

        private List<PortfolioStockViewModel> GetPortfolioStocksForPortfolioAsset(int portfolioAssetId)
        {
            List<PortfolioStockViewModel> portfolioStocks = new List<PortfolioStockViewModel>();
            var response = _httpClient.GetAsync(_httpClient.BaseAddress + "/PortfolioStock/GetPortfolioStocksForPortfolioAsset/" + portfolioAssetId).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                portfolioStocks = JsonConvert.DeserializeObject<List<PortfolioStockViewModel>>(data);
            }

            return portfolioStocks;
        }
    }
}
