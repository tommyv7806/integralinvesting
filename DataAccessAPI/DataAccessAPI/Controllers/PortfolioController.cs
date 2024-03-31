using DataAccessAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DataAccessAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PortfolioController : ControllerBase
    {
        private static readonly List<Portfolio> _portfolios = Portfolio.GetPortfolios();
        private readonly ILogger<PortfolioController> _logger;

        public PortfolioController(ILogger<PortfolioController> logger) => _logger = logger;

        [HttpGet(Name = "GetPortfolioList")]
        public IEnumerable<Portfolio> Get() => Portfolio.GetPortfolios();
    }
}
