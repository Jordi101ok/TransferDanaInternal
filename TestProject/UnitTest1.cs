using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TransferDanaInternal.Controllers;
using TransferDanaInternal.Models;
using Xunit;

namespace TestProject
{
    public class TransferControllerTests
    {
        private readonly TransferController _controller;

        public TransferControllerTests()
        {
            var logger = new LoggerFactory().CreateLogger<TransferController>();
            _controller = new TransferController(logger);

            TransferController.accounts.First(a => a.AccountNumber == "543241").Balance = 1000000;
            TransferController.accounts.First(a => a.AccountNumber == "772851").Balance = 500000;
        }

        // success, transaction result valid
        [Fact]
        public void Transfer_ValidRequest_ReturnsCorrectFormModel()
        {
            var result = _controller.Index("543241", "902295", 50000, "Test transfer") as ViewResult;
            var form = result.Model as TransferForm;

            Assert.NotNull(form);
            Assert.Equal("543241", form.SourceAccount.AccountNumber);
            Assert.Equal("902295", form.DestinationAccount.AccountNumber);
            Assert.Equal(50000, form.Amount);
            Assert.Equal("Test transfer", form.Description);
        }

        // success, source balance decrease
        [Fact]
        public void Transfer_ValidRequest_SourceBalanceDecreases()
        {
            _controller.Index("543241", "902295", 50000, "Test");

            var source = TransferController.accounts.First(a => a.AccountNumber == "543241");
            Assert.Equal(950000, source.Balance);
        }

        // fail, amount = 0
        [Fact]
        public void Transfer_ZeroAmount_ReturnsBadRequest()
        {
            var result = _controller.Index("543241", "902295", 0, "Test") as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal("Amount must be greater than 0.", result.Value);
        }

        // fail, amount = negative
        [Fact]
        public void Transfer_NegativeAmount_ReturnsBadRequest()
        {
            var result = _controller.Index("543241", "902295", -100, "Test") as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal("Amount must be greater than 0.", result.Value);
        }

        // fail, amount > balance
        [Fact]
        public void Transfer_AmountExceedsBalance_ReturnsBadRequest()
        {
            var result = _controller.Index("543241", "902295", 9999999, "Test") as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal("Amount exceeds source account balance (Rp 1,000,000).", result.Value);
        }
    }
}