using DataAccessWebAPI.DataAccessLayer;
using DataAccessWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataAccessWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PortfolioAssetController : ControllerBase
    {
        private readonly IntegralInvestingAppDbContext _context;

        public PortfolioAssetController(IntegralInvestingAppDbContext context) { _context = context; }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var portfolioAssets = _context.PortfolioAssets.ToList();

                if (portfolioAssets.Count == 0)
                    return NotFound("Portfolio Assets not available");

                return Ok(portfolioAssets);
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
                var portfolioAssets = _context.PortfolioAssets.Find(id);

                if (portfolioAssets == null)
                    return NotFound($"Portfolio Asset details not found with Id of {id}.");

                return Ok(portfolioAssets);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{portfolioId}")]
        public IActionResult GetPortfolioAssetsForUserPortfolio(int portfolioId)
        {
            try
            {
                var portfolioAssets = _context.PortfolioAssets.Where(ps => ps.PortfolioId == portfolioId).ToList();

                if (portfolioAssets.Count == 0)
                    return NotFound($"Portfolio Assets not available for Portfolio with PortfolioId of '{portfolioId}'");

                return Ok(portfolioAssets);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{symbol}")]
        public IActionResult GetPortfolioAssetForStockSymbol(string symbol)
        {
            try
            {
                var portfolioAsset = _context.PortfolioAssets
                    .Include(pa => pa.PortfolioStocks)
                    .SingleOrDefault(pa => pa.Symbol.ToLower() == symbol.ToLower());

                if (portfolioAsset == null)
                    return NotFound($"Portfolio Assets not available for Portfolio with Symbol of '{symbol}'");

                return Ok(portfolioAsset);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{symbol},{portfolioId}")]
        public IActionResult GetPortfolioAssetForStockSymbolFromPortfolio(string symbol, int portfolioId)
        {
            try
            {
                var portfolioAsset = _context.PortfolioAssets
                    .Include(pa => pa.PortfolioStocks)
                    .SingleOrDefault(pa => pa.Symbol.ToLower() == symbol.ToLower() && pa.PortfolioId == portfolioId);

                if (portfolioAsset == null)
                    return NotFound($"Portfolio Assets not available for Portfolio with Symbol of '{symbol}'");

                return Ok(portfolioAsset);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public IActionResult Put(PortfolioAsset model)
        {
            if (model == null || model.PortfolioAssetId == 0)
            {
                if (model == null)
                    return BadRequest("Portfolio Asset data is invalid");
                else if (model.PortfolioId == 0)
                    return BadRequest($"Portfolio Asset Id {model.PortfolioAssetId} is invalid");
            }

            try
            {
                var portfolioAsset = _context.PortfolioAssets.Find(model.PortfolioAssetId);

                if (portfolioAsset == null)
                    return BadRequest($"Portfolio Asset not found with Id of {model.PortfolioAssetId}");

                portfolioAsset.PortfolioId = model.PortfolioId;
                portfolioAsset.CurrentPrice = model.CurrentPrice;
                portfolioAsset.Symbol = model.Symbol;
                portfolioAsset.Name = model.Name;

                _context.SaveChanges();
                return Ok("Portfolio Asset details updated");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public IActionResult Post(PortfolioAsset model)
        {
            try
            {
                _context.Add(model);
                _context.SaveChanges();
                return Ok("Portfolio Asset successfully created");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var portfolioAsset = _context.PortfolioAssets.Find(id);

                if (portfolioAsset == null)
                    return NotFound($"Portfolio Asset not found with Id of {id}");

                _context.PortfolioAssets.Remove(portfolioAsset);
                _context.SaveChanges();

                return Ok("Portfolio Asset removed");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
