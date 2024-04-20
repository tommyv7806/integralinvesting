using DataAccessWebAPI.DataAccessLayer;
using DataAccessWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataAccessWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BankAccountController : ControllerBase
    {
        private readonly IntegralInvestingAppDbContext _context;

        public BankAccountController(IntegralInvestingAppDbContext context) { _context = context; }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var bankAccounts = _context.BankAccounts.ToList();

                if (bankAccounts.Count == 0)
                    return NotFound("Bank Accounts not available");

                return Ok(bankAccounts);
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
                var bankAccount = _context.BankAccounts.Find(id);

                if (bankAccount == null)
                    return NotFound($"Bank Account details not found with Id of {id}.");

                return Ok(bankAccount);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{userId}")]
        public IActionResult GetUserAccounts(string userId)
        {
            try
            {
                var bankAccounts = _context.BankAccounts.Where(b => b.UserId == userId).ToList();

                if (bankAccounts.Count == 0)
                    return NotFound($"Bank Accounts not available for user with UserId of '{userId}'");

                return Ok(bankAccounts);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public IActionResult Post(BankAccount model)
        {
            try
            {
                _context.Add(model);
                _context.SaveChanges();
                return Ok("Bank Account successfully created");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public IActionResult Put(BankAccount model)
        {
            if (model == null || model.BankAccountId == 0)
            {
                if (model == null)
                    return BadRequest("Bank Account data is invalid");
                else if (model.BankAccountId == 0)
                    return BadRequest($"Bank Account Id {model.BankAccountId} is invalid");
            }

            try
            {
                var bankAccount = _context.BankAccounts.Find(model.BankAccountId);

                if (bankAccount == null)
                    return BadRequest($"Bank Account not found with Id of {model.BankAccountId}");

                bankAccount.UserId = model.UserId;
                bankAccount.AccountName = model.AccountName;

                _context.SaveChanges();
                return Ok("Bank Account details updated");
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
                var bankAccount = _context.BankAccounts.Find(id);

                if (bankAccount == null)
                    return NotFound($"Bank Account not found with Id of {id}");

                _context.BankAccounts.Remove(bankAccount);
                _context.SaveChanges();

                return Ok("Bank Account removed");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
