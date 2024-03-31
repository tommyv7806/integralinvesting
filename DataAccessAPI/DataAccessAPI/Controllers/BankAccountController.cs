using DataAccessAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DataAccessAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankAccountController : ControllerBase
    {
        private static readonly List<BankAccount> _bankAccounts = BankAccount.GetBankAccounts();
        private readonly ILogger<BankAccountController> _logger;

        public BankAccountController(ILogger<BankAccountController> logger) => _logger = logger;

        [HttpGet(Name = "GetBankAccountList")]
        public IEnumerable<BankAccount> Get() => BankAccount.GetBankAccounts();
    }
}
