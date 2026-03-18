using Microsoft.AspNetCore.Mvc;
using RESTAPITransaksi_Rekening.Models;

namespace RESTAPITransaksi_Rekening.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionsController : ControllerBase
    {
        private static readonly List<Transaction> _transactions = new();

        // POST /api/transactions
        [HttpPost]
        public ActionResult<Transaction> CreateTransaction([FromBody] Transaction request)
        {
            request.TransactionId = Guid.NewGuid();
            _transactions.Add(request);
            return CreatedAtAction(nameof(GetLastTransaction), new { accountnumberfrom = request.AccountNumberFrom }, request);
        }

        // GET /api/transactions?accountnumberfrom=...
        [HttpGet]
        public ActionResult<Transaction> GetLastTransaction([FromQuery] string accountnumberfrom)
        {
            if (string.IsNullOrWhiteSpace(accountnumberfrom))
            {
                return BadRequest("accountnumberfrom query parameter is required.");
            }

            var transaction = _transactions
                .Where(t => t.AccountNumberFrom == accountnumberfrom)
                .OrderByDescending(t => t.Time)
                .FirstOrDefault();

            if (transaction is null)
            {
                return NotFound($"No transactions found for account number '{accountnumberfrom}'.");
            }                

            return Ok(transaction);
        }
    }
}
