using DataAccessAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DataAccessAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserAccountController : ControllerBase
    {
        private static readonly List<UserAccount> _userAccounts = UserAccount.GetUserAccounts();
        private readonly ILogger<UserAccountController> _logger;

        public UserAccountController(ILogger<UserAccountController> logger) => _logger = logger;

        [HttpGet(Name = "GetUserAccountList")]
        public IEnumerable<UserAccount> Get() => UserAccount.GetUserAccounts();
    }
}
