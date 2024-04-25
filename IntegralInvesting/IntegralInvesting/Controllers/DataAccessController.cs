using IntegralInvesting.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace IntegralInvesting.Controllers
{
    public class DataAccessController : Controller
    {
        // ====== USER FUND RELATED ====== //
        public UserFundViewModel GetFundsForCurrentUser(string currentUserId, HttpClient httpClient)
        {
            UserFundViewModel userFund = new UserFundViewModel();
            var response = httpClient.GetAsync(httpClient.BaseAddress + "/UserFund/GetUserFunds/" + currentUserId).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                userFund = JsonConvert.DeserializeObject<List<UserFundViewModel>>(data).Single();
            }

            return userFund;
        }

        public void UpdateUserCurrentFunds(UserFundViewModel userFunds, HttpClient httpClient)
        {
            try
            {
                string data = JsonConvert.SerializeObject(userFunds);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                var stringContent = content.ReadAsStringAsync().Result;
                HttpResponseMessage response = httpClient.PutAsync(httpClient.BaseAddress + "/UserFund/Put", content).Result;
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }
        }

        // ====== PORTFOLIO ASSET RELATED ====== //
        public PortfolioAssetViewModel GetSelectedPortfolioAsset(string symbol, HttpClient httpClient)
        {
            PortfolioAssetViewModel portfolioAsset = new PortfolioAssetViewModel();
            var response = httpClient.GetAsync(httpClient.BaseAddress + "/PortfolioAsset/GetPortfolioAssetForStockSymbol/" + symbol).Result;

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

        public List<PortfolioStockViewModel> GetPortfolioStocksForPortfolioAsset(int portfolioAssetId, HttpClient httpClient)
        {
            List<PortfolioStockViewModel> portfolioStocks = new List<PortfolioStockViewModel>();
            var response = httpClient.GetAsync(httpClient.BaseAddress + "/PortfolioStock/GetPortfolioStocksForPortfolioAsset/" + portfolioAssetId).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                portfolioStocks = JsonConvert.DeserializeObject<List<PortfolioStockViewModel>>(data);
            }

            return portfolioStocks;
        }

        public void DeletePortfolioAsset(int id, HttpClient httpClient)
        {
            var response = httpClient.DeleteAsync(httpClient.BaseAddress + "/PortfolioAsset/Delete/" + id).Result;
        }

        // ====== PORTFOLIO STOCK RELATED ====== //
        public void UpdatePortfolioStock(PortfolioStockViewModel portfolioStock, HttpClient httpClient)
        {
            try
            {
                string data = JsonConvert.SerializeObject(portfolioStock);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                var stringContent = content.ReadAsStringAsync().Result;
                HttpResponseMessage response = httpClient.PutAsync(httpClient.BaseAddress + "/PortfolioStock/Put", content).Result;
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }
        }

        public void DeletePortfolioStock(int id, HttpClient httpClient)
        {
            try
            {
                var response = httpClient.DeleteAsync(httpClient.BaseAddress + "/PortfolioStock/Delete/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Do something
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }
        }

        // ====== PORTFOLIO RELATED ====== //
        public PortfolioViewModel GetPortfolioForCurrentUser(string currentUserId, HttpClient httpClient)
        {
            PortfolioViewModel portfolio = new PortfolioViewModel();
            var response = httpClient.GetAsync(httpClient.BaseAddress + "/Portfolio/GetUserPortfolio/" + currentUserId).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                portfolio = JsonConvert.DeserializeObject<List<PortfolioViewModel>>(data).Single();
            }

            return portfolio;
        }
    }
}
