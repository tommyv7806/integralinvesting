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

        public PurchaseStockController(UserManager<IntegralInvestingUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        [HttpGet]
        public IActionResult Index(string searchString)
        {
            if (searchString != null)
            {
                ViewData["SearchString"] = searchString;
            }
            return View();
        }

        public IActionResult Search(string searchString)
        {
            if (searchString != null)
            {
                // API TEST SECTION
                var symbol = searchString;
                var apiKey = _config.GetValue<string>("AlphaVantageSettings:ApiKey:Key");

                var stockApiResponse = $"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={symbol}&apikey={apiKey}&interval=60min&datatype=csv"
                    .GetStringFromUrl();

                if (stockApiResponse.Contains("Invalid API call"))
                {
                    ViewData["ErrorMessage"] = "Please enter a valid stock symbol (e.g., MSFT, AAPL, etc.)";
                    return PartialView("PurchaseSearchResultPartial", new List<PurchaseStockViewModel>());
                }

                var allPrices = stockApiResponse.FromCsv<List<PurchaseStockViewModel>>().ToList();

                /////////////////////

                return PartialView("PurchaseSearchResultPartial", allPrices);
            }
            

            return PartialView("PurchaseSearchResultPartial", new List<PurchaseStockViewModel>());
        }

        public IActionResult TestSearch(string searchString)
        {
            if (searchString != null)
            {
                // API TEST SECTION
                var symbol = searchString;
                var apiKey = _config.GetValue<string>("AlphaVantageSettings:ApiKey:Key");

                var stockApiResponse = $"https://www.alphavantage.co/query?function=SYMBOL_SEARCH&keywords={symbol}&apikey={apiKey}&datatype=csv"
                    .GetStringFromUrl();

                if (stockApiResponse.Contains("Invalid API call"))
                {
                    ViewData["ErrorMessage"] = "Please enter a valid stock symbol (e.g., MSFT, AAPL, etc.)";
                    return PartialView("TestPurchaseSearchResultPartial", new List<StockSearchViewModel>());
                }

                var results = stockApiResponse.FromCsv<List<StockSearchViewModel>>().ToList();

                /////////////////////

                return PartialView("TestPurchaseSearchResultPartial", results);
            }


            return PartialView("TestPurchaseSearchResultPartial", new List<StockSearchViewModel>());
        }
    }
}
