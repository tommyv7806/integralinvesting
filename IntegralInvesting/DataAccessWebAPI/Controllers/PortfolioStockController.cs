using DataAccessWebAPI.DataAccessLayer;
using DataAccessWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DataAccessWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PortfolioStockController : ControllerBase
    {
        private readonly IntegralInvestingAppDbContext _context;

        public PortfolioStockController(IntegralInvestingAppDbContext context) { _context = context; }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var portfolioStocks = _context.PortfolioStocks.ToList();

                if (portfolioStocks.Count == 0)
                    return NotFound("Portfolio Stocks not available");

                return Ok(portfolioStocks);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var portfolioStocks = _context.PortfolioStocks.Find(id);

                if (portfolioStocks == null)
                    return NotFound($"Portfolio Stock details not found with Id of {id}.");

                return Ok(portfolioStocks);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //[HttpGet("{portfolioId}")]
        //public IActionResult GetPortfolioStocksForUserPortfolio(int portfolioId)
        //{
        //    try
        //    {
        //        var portfolioStocks = _context.PortfolioStocks.Where(ps => ps.PortfolioId == portfolioId).ToList();

        //        if (portfolioStocks.Count == 0)
        //            return NotFound($"Portfolio Stocks not available for Portfolio with PortfolioId of '{portfolioId}'");

        //        return Ok(portfolioStocks);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        [HttpPut]
        public IActionResult Put(PortfolioStock model)
        {
            if (model == null || model.PortfolioStockId == 0)
            {
                if (model == null)
                    return BadRequest("Portfolio Stock data is invalid");
                else if (model.PortfolioStockId == 0)
                    return BadRequest($"Portfolio Stock Id {model.PortfolioStockId} is invalid");
            }

            try
            {
                var portfolioStock = _context.PortfolioStocks.Find(model.PortfolioStockId);

                if (portfolioStock == null)
                    return BadRequest($"Portfolio Stock not found with Id of {model.PortfolioStockId}");

                portfolioStock.PortfolioAssetId = model.PortfolioAssetId;
                portfolioStock.PurchasePrice = model.PurchasePrice;
                portfolioStock.CurrentPrice = model.CurrentPrice;
                portfolioStock.PurchaseQuantity = model.PurchaseQuantity;

                _context.SaveChanges();
                return Ok("Portfolio Stock details updated");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public IActionResult Post(PortfolioStock model)
        {
            try
            {
                _context.Add(model);
                _context.SaveChanges();
                return Ok("Portfolio Stock successfully created");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var portfolioStock = _context.PortfolioStocks.Find(id);

                if (portfolioStock == null)
                    return NotFound($"Portfolio Stock not found with Id of {id}");

                _context.PortfolioStocks.Remove(portfolioStock);
                _context.SaveChanges();

                return Ok("Portfolio Stock removed");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
