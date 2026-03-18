using Microsoft.AspNetCore.Mvc;
using TransferDanaInternal.Models;

namespace TransferDanaInternal.Controllers
{
    public class TransferController : Controller
    {
        private readonly ILogger<TransferController> _logger;

        public TransferController(ILogger<TransferController> logger)
        {
            _logger = logger;
        }

        public static List<Account> accounts = new List<Account>
        {
            new Account { AccountNumber = "543241", AccountName = "Main Account", Balance = 1000000 },
            new Account { AccountNumber = "772851", AccountName = "Savings", Balance = 500000 },

            new Account { AccountNumber = "902295", AccountName = "John", Balance = 0 },
            new Account { AccountNumber = "966742", AccountName = "Doe", Balance = 0 },
            new Account { AccountNumber = "202115", AccountName = "Alice", Balance = 0 },
            new Account { AccountNumber = "783331", AccountName = "Bob", Balance = 0 },
            new Account { AccountNumber = "542150", AccountName = "Charlie", Balance = 0 }
        };

        [HttpGet]
        public IActionResult Index()
        {
            var user = HttpContext.Session.GetString("User");

            if (string.IsNullOrEmpty(user))
            {
                return RedirectToAction("Index", "Login");
            }

            _logger.LogInformation("Transfer page accessed by user: {User}", user);

            var model = new TransferViewModel
            {
                Form = new TransferForm(),
                SourceAccounts = accounts.Take(2).ToList(),
                DestinationAccounts = accounts.Skip(2).Take(5).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string sourceAccountNumber, string destinationAccountNumber, double amount, string description)
        {
            // source account empty
            if (string.IsNullOrEmpty(sourceAccountNumber))
            {
                _logger.LogWarning("Transfer failed. Source account is not selected.");
                return BadRequest("Please select a source account.");
            }

            // destination account empty
            if (string.IsNullOrEmpty(destinationAccountNumber))
            {
                _logger.LogWarning("Transfer failed. Destination account is not selected.");
                return BadRequest("Please select a destination account.");
            }

            var sourceAccount = accounts.FirstOrDefault(a => a.AccountNumber == sourceAccountNumber);
            var destinationAccount = accounts.FirstOrDefault(a => a.AccountNumber == destinationAccountNumber);

            // amount <= 0
            if (amount <= 0)
            {
                _logger.LogWarning("Transfer failed. Amount must be greater than 0. Amount: {Amount}", amount);
                return BadRequest("Amount must be greater than 0.");
            }

            // Amount > balance
            if (amount > sourceAccount.Balance)
            {
                _logger.LogWarning("Transfer failed. Amount {Amount} exceeds balance {Balance} for account {Source}",
                    amount, sourceAccount.Balance, sourceAccountNumber);
                return BadRequest($"Amount exceeds source account balance (Rp {sourceAccount.Balance.ToString("N0")}).");
            }

            // Description empty
            if (string.IsNullOrEmpty(description))
            {
                _logger.LogWarning("Transfer failed. Description is empty.");
                return BadRequest("Description is required.");
            }

            // Description > 40 characters
            if (description.Length > 40)
            {
                _logger.LogWarning("Transfer failed. Description exceeds 40 characters. Length: {Length}", description.Length);
                return BadRequest("Description must not exceed 40 characters.");
            }

            sourceAccount.Balance -= amount;

            var form = new TransferForm
            {
                SourceAccount = sourceAccount,
                DestinationAccount = destinationAccount,
                Amount = amount,
                Description = description
            };

            return View("TransactionResult", form);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
