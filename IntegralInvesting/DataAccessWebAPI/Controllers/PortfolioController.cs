using Microsoft.AspNetCore.Mvc;
using DataAccessWebAPI.DataAccessLayer;
using DataAccessWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly IntegralInvestingAppDbContext _context;

        public PortfolioController(IntegralInvestingAppDbContext context) { _context = context; }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var portfolios = _context.Portfolios.ToList();

                if (portfolios.Count == 0)
                    return NotFound("Portfolios not available");

                return Ok(portfolios);
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
                var portfolio = _context.Portfolios.Find(id);

                if (portfolio == null)
                    return NotFound($"Portfolio details not found with Id of {id}.");

                return Ok(portfolio);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{userId}")]
        public IActionResult GetUserPortfolio(string userId)
        {
            try
            {
                var portfolio = _context.Portfolios
                    .Include(p => p.PortfolioStocks)
                    .Where(p => p.UserId == userId)
                    .ToList();

                if (portfolio.Count == 0)
                    return NotFound($"Portfolio not available for user with UserId of '{userId}'");

                return Ok(portfolio);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //[HttpPut]
        //public IActionResult Put(Portfolio model)
        //{
        //    if (model == null || model.PortfolioId == 0)
        //    {
        //        if (model == null)
        //            return BadRequest("Portfolio data is invalid");
        //        else if (model.PortfolioId == 0)
        //            return BadRequest($"Portfolio Id {model.PortfolioId} is invalid");
        //    }

        //    try
        //    {
        //        var portfolio = _context.Portfolios.Find(model.PortfolioId);

        //        if (portfolio == null)
        //            return BadRequest($"Portfolio not found with Id of {model.PortfolioId}");

        //        portfolio.UserId = model.UserId;

        //        _context.SaveChanges();
        //        return Ok("Portfolio details updated");
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        [HttpPost]
        public IActionResult Post(Portfolio model)
        {
            try
            {
                _context.Add(model);
                _context.SaveChanges();
                return Ok("Portfolio successfully created");
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
                var portfolio = _context.Portfolios.Find(id);

                if (portfolio == null)
                    return NotFound($"Portfolio not found with Id of {id}");

                _context.Portfolios.Remove(portfolio);
                _context.SaveChanges();

                return Ok("Portfolio removed");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
