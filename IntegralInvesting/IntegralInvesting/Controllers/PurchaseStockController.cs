﻿using IntegralInvesting.Areas.Identity.Data;
using IntegralInvesting.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.ContentModel;
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
        public IActionResult Index()
        {
            var searchQuery = HttpContext.Session.GetString("SearchQuery");
            var symbol = HttpContext.Session.GetString("Symbol");
            var stockName = HttpContext.Session.GetString("StockName");

            ViewData["SearchQuery"] = searchQuery;
            ViewData["Symbol"] = symbol;
            ViewData["StockName"] = stockName;

            return View();
        }

        // After the user clicks the search button, display the initial search results on left side of screen
        public IActionResult InitialSearch(string searchString)
        {
            HttpContext.Session.SetString("SearchQuery", searchString);

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
                var stockDetails = GetBasicStockDetails(symbol);

                HttpContext.Session.SetString("Symbol", symbol);
                HttpContext.Session.SetString("StockName", stockName);

                ViewData["Symbol"] = symbol;
                ViewData["StockName"] = stockName;
                ViewData["Price"] = stockDetails.Price;

                var timeDetails = GetStockTimeDetails(symbol);

                stockDetails.StockTimeDetailsList.AddRange(timeDetails);

                return PartialView("SearchResultDetailsPartial", stockDetails);
            }
            
            return PartialView("SearchResultDetailsPartial", new StockDetailsViewModel());
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
        public IActionResult PurchaseShares(PortfolioStockViewModel portfolioStock)
        {
            var currentUserId = _userManager.GetUserId(this.User);
            var userPortfolio = GetPortfolioForCurrentUser(currentUserId);

            var userFunds = GetFundsForCurrentUser(currentUserId);
            userFunds.CurrentFunds -= portfolioStock.PurchaseTotal;

            UpdateUserCurrentFunds(userFunds);
            CreateOrUpdatePortfolioAsset(portfolioStock, userPortfolio);

            return CreateNewPortfolioStock(portfolioStock);
        }

        private void CreateOrUpdatePortfolioAsset(PortfolioStockViewModel portfolioStock, PortfolioViewModel portfolio)
        {
            var portfolioAsset = portfolio.PortfolioAssets.FirstOrDefault(pa => pa.Symbol == portfolioStock.Symbol);

            if (portfolioAsset != null)
            {
                portfolioStock.PortfolioAssetId = portfolioAsset.PortfolioAssetId;
            }
            else
            {
                portfolioAsset = new PortfolioAssetViewModel
                {
                    Name = portfolioStock.Name,
                    Symbol = portfolioStock.Symbol,
                    PortfolioId = portfolio.PortfolioId,
                    NumberOfShares = 0
                };

                CreateNewPortfolioAsset(portfolioAsset);

                portfolio.PortfolioAssets.Add(portfolioAsset);
            }
        }

        private IActionResult CreateNewPortfolioStock(PortfolioStockViewModel portfolioStock)
        {
            var currentUserId = _userManager.GetUserId(this.User);
            var userPortfolio = GetPortfolioForCurrentUser(currentUserId);

            var existingPortfolioAsset = userPortfolio.PortfolioAssets.First(pa => pa.Symbol == portfolioStock.Symbol);

            portfolioStock.PortfolioAssetId = existingPortfolioAsset.PortfolioAssetId;

            try
            {
                string data = JsonConvert.SerializeObject(portfolioStock);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/PortfolioStock/Post", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = $"{portfolioStock.PurchaseQuantity} shares successfully purchased for {portfolioStock.Name}";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                return PartialView("PurchaseSharesModalPartial", portfolioStock);
            }

            return PartialView("PurchaseSharesModalPartial", portfolioStock);
        }

        private void CreateNewPortfolioAsset(PortfolioAssetViewModel portfolioAsset)
        {
            try
            {
                string data = JsonConvert.SerializeObject(portfolioAsset);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/PortfolioAsset/Post", content).Result;
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }
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

        private List<StockTimeDetails> GetStockTimeDetails(string symbol)
        {
            var stockApiResponse = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&apikey={_apiKey}&datatype=csv"
                    .GetStringFromUrl();

            var results = stockApiResponse.FromCsv<List<StockTimeDetails>>().ToList();

            var lastSevenDaysData = results.Take(7).Reverse().ToList();

            return lastSevenDaysData;
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
