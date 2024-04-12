using DataAccessWebAPI.DataAccessLayer;
using DataAccessWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DataAccessWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserFundController : ControllerBase
    {
        private readonly IntegralInvestingAppDbContext _context;
        public UserFundController(IntegralInvestingAppDbContext context) { _context = context; }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var userFunds = _context.UserFunds.ToList();

                if (userFunds.Count == 0)
                    return NotFound("User Funds not available");

                return Ok(userFunds);
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
                var userFund = _context.UserFunds.Find(id);

                if (userFund == null)
                    return NotFound($"User Fund details not found with Id of {id}.");

                return Ok(userFund);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{userId}")]
        public IActionResult GetUserFunds(string userId)
        {
            try
            {
                var userFunds = _context.UserFunds.Where(u => u.UserId == userId).ToList();

                if (userFunds.Count == 0)
                    return NotFound($"User Funds not available for User Id of {userId}");

                return Ok(userFunds);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public IActionResult Post(UserFund model)
        {
            try
            {
                _context.Add(model);
                _context.SaveChanges();
                return Ok("User Fund successfully created");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public IActionResult Put(UserFund model)
        {
            if (model == null || model.UserFundId == 0)
            {
                if (model == null)
                    return BadRequest("user Fund data is invalid");
                else if (model.UserFundId == 0)
                    return BadRequest($"User Fund Id {model.UserFundId} is invalid");
            }

            try
            {
                var userFund = _context.BankAccounts.Find(model.UserFundId);

                if (userFund == null)
                    return BadRequest($"User Fund not found with Id of {model.UserFundId}");

                userFund.CurrentFunds = model.CurrentFunds; // Should really only be updating CurrentFunds property

                _context.SaveChanges();
                return Ok("User Fund details updated");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // Set this up, but will likely not need this
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var userFund = _context.UserFunds.Find(id);

                if (userFund == null)
                    return NotFound($"User Fund not found with Id of {id}");

                _context.UserFunds.Remove(userFund);
                _context.SaveChanges();

                return Ok("User Fund removed");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
    
}
        

    

