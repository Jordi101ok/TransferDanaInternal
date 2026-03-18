using Microsoft.AspNetCore.Mvc;
using RESTAPITransaksi_Rekening.Models;

namespace RESTAPITransaksi_Rekening.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountsController : ControllerBase
    {
        private static readonly Dictionary<string, Account> _accounts = new();

        // POST /api/accounts
        [HttpPost]
        public IActionResult CreateAccount([FromBody] Account account)
        {
            if (string.IsNullOrWhiteSpace(account.Name))
            {
                return BadRequest("Name is required.");
            }
                
            if (string.IsNullOrWhiteSpace(account.Number))
            {
                return BadRequest("Number is required.");
            }
                
            if (_accounts.ContainsKey(account.Number))
            {
                return Conflict($"Account with number '{account.Number}' already exists.");
            }
                
            _accounts[account.Number] = account;

            return CreatedAtAction(nameof(GetAccount), new { number = account.Number }, account);
        }

        // GET /api/accounts/{number}
        [HttpGet("{number}")]
        public IActionResult GetAccount(string number)
        {
            if (!_accounts.TryGetValue(number, out var account))
                return NotFound($"Account with number '{number}' not found.");

            return Ok(account);
        }
    }
}
