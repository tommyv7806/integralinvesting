using DataAccessAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DataAccessAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PortfolioStockController : ControllerBase
    {
        private static readonly List<PortfolioStock> _portfolioStocks = PortfolioStock.GetPortfolioStocks();
        private readonly ILogger<PortfolioStockController> _logger;

        public PortfolioStockController(ILogger<PortfolioStockController> logger) => _logger = logger;

        [HttpGet(Name = "GetPortfolioStocksList")]
        public IEnumerable<PortfolioStock> Get() => PortfolioStock.GetPortfolioStocks();
    }
}
