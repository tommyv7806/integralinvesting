using IntegralInvesting.Areas.Identity.Data;
using IntegralInvesting.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        private DataAccessController _dataAccess;

        public PortfolioController(UserManager<IntegralInvestingUser> userManager, IConfiguration config)
        {
            _httpClient = new HttpClient();
            _dataAccess = new DataAccessController();
            _httpClient.BaseAddress = baseAddress;
            _userManager = userManager;
            _config = config;
        }

        // Display the information for the user's Portfolio
        public ActionResult Index()
        {
            ValidateUserIsLoggedIn();
            var currentUserId = _userManager.GetUserId(this.User);
            var userPortfolio = _dataAccess.GetPortfolioForCurrentUser(currentUserId, _httpClient);

            foreach (var asset in userPortfolio.PortfolioAssets)
            {
                var stockDetails = GetBasicStockDetails(asset.Symbol);
                asset.CurrentPrice = stockDetails.Price;
            }

            return View(userPortfolio.PortfolioAssets);
        }

        // Opens the modal where users can enter the number of shares of a particular stock they want to sell
        [HttpGet]
        public IActionResult OpenSellModal(string symbol, decimal currentPrice, int numberOfShares)
        {
            var portfolioAsset = _dataAccess.GetSelectedPortfolioAsset(symbol, _httpClient);
            portfolioAsset.NumberOfShares = numberOfShares;
            portfolioAsset.CurrentPrice = currentPrice;

            return PartialView("SellSharesModalPartial", portfolioAsset);
        }

        [HttpPost]
        public IActionResult SellShares(PortfolioAssetViewModel portfolioAsset)
        {
            ValidateUserIsLoggedIn();
            var currentUserId = _userManager.GetUserId(this.User);

            var userFunds = _dataAccess.GetFundsForCurrentUser(currentUserId, _httpClient);
            userFunds.CurrentFunds += portfolioAsset.SaleTotal;
            _dataAccess.UpdateUserCurrentFunds(userFunds, _httpClient);


            var portfolioStocks = _dataAccess.GetPortfolioStocksForPortfolioAsset(portfolioAsset.PortfolioAssetId, _httpClient);
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

                        _dataAccess.UpdatePortfolioStock(stock, _httpClient);
                    }
                    // If num of shares for stock is less than sell qty, reduce num of shares to zero, delete PortfolioStock record, and subtract num of shares from sell qty
                    else if (stock.PurchaseQuantity <= sellQuantity)
                    {
                        sellQuantity -= stock.PurchaseQuantity;

                        // Delete PortfolioStock record
                        DeletePortfolioStock(stock);
                    }
                }
            }

            return RedirectToAction("Index");
        }

        private void DeletePortfolioStock(PortfolioStockViewModel portfolioStock)
        {
            try
            {
                _dataAccess.DeletePortfolioStock(portfolioStock.PortfolioStockId, _httpClient);

                var portfolioAsset = _dataAccess.GetSelectedPortfolioAsset(portfolioStock.Symbol, _httpClient);

                if (portfolioAsset != null && portfolioAsset.PortfolioStocks.Count() == 0)
                    _dataAccess.DeletePortfolioAsset(portfolioStock.PortfolioAssetId, _httpClient);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }
        }

        private StockDetailsViewModel GetBasicStockDetails(string symbol)
        {
            var apiKey = _config.GetValue<string>("AlphaVantageSettings:ApiKey:Key");

            var stockApiResponse = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={symbol}&apikey={apiKey}&datatype=csv"
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
    }
}
