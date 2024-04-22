using IntegralInvesting.Areas.Identity.Data;
using IntegralInvesting.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceStack;

namespace IntegralInvesting.Controllers
{
    public class PurchaseStockController : Controller
    {
        private readonly UserManager<IntegralInvestingUser> _userManager;
        private readonly IConfiguration _config;
        private readonly string _apiKey;

        public PurchaseStockController(UserManager<IntegralInvestingUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;

            _apiKey = _config.GetValue<string>("AlphaVantageSettings:ApiKey:Key");
        }

        [HttpGet]
        public IActionResult Index(string searchString)
        {
            return View();
        }

        public IActionResult DetailsSearch(string symbol)
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

                return PartialView("SearchResultDetailsPartial", allPrices);
            }
            
            return PartialView("SearchResultDetailsPartial", new List<StockDetailsViewModel>());
        }

        public IActionResult InitialSearch(string searchString)
        {
            if (searchString != null)
            {
                var stockApiResponse = $"https://www.alphavantage.co/query?function=SYMBOL_SEARCH&keywords={searchString}&apikey={_apiKey}&datatype=csv"
                    .GetStringFromUrl();

                if (stockApiResponse.Contains("Invalid API call"))
                {
                    ViewData["ErrorMessage"] = "Please enter a valid stock symbol (e.g., MSFT, AAPL, etc.)";
                    return PartialView("InitialSearchResultPartial", new List<StockSearchViewModel>());
                }

                var results = stockApiResponse.FromCsv<List<StockSearchViewModel>>().ToList();

                return PartialView("InitialSearchResultPartial", results);
            }

            return PartialView("InitialSearchResultPartial", new List<StockSearchViewModel>());
        }
    }
}
