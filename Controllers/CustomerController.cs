using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MCBA_Web.Data;
using MCBA_Web.Models;
using MCBA_Web.Utilities;
using MCBA_Web.Filters;
using MCBA_Web.ViewModels;

namespace MCBA_Web.Controllers
{
    // Can add authorize attribute to controllers.
    [AuthorizeCustomer]
    public class CustomerController : Controller
    {
        private readonly McbaContext _context;

        // ReSharper disable once PossibleInvalidOperationException
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public CustomerController(McbaContext context) => _context = context;

        // Can add authorize attribute to actions.
        [AuthorizeCustomer]
        public async Task<IActionResult> Index()
        {
            // Lazy loading.
            // The Customer.Accounts property will be lazy loaded upon demand.
            var customer = await _context.Customers.FindAsync(CustomerID);

            // OR
            // Eager loading.
            //var customer = await _context.Customers.Include(x => x.Accounts).
            //    FirstOrDefaultAsync(x => x.CustomerID == _customerID);

            return View(customer);
        }

        public async Task<IActionResult> Deposit(int id)
        {
            return View(
                new DepositViewModel
                {
                    AccountNumber = id,
                    Account = await _context.Accounts.FindAsync(id)
                });
        }
        public bool verifyAmount(decimal amount)
        {
            if (amount <= 0)
            {
                ModelState.AddModelError(nameof(amount), "Amount must be positive.");
                return false;
            }
            if (amount.HasMoreThanTwoDecimalPlaces())
            {
                ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
                return false;
            }
            return true;
        }
        [HttpPost]
        public async Task<IActionResult> Deposit(DepositViewModel viewModel)
        {
            viewModel.Account = await _context.Accounts.FindAsync(viewModel.AccountNumber);

            if (!verifyAmount(viewModel.Amount))
            {
                return View(viewModel);
            }
            // Note this code could be moved out of the controller, e.g., into the model or repository (design pattern).
            viewModel.Account.Balance += viewModel.Amount;
            viewModel.Account.Transactions.Add(
                new Transaction
                {
                    TransactionType = (char)TransactionType.Deposit,
                    Amount = viewModel.Amount,
                    Comment = viewModel.Comment,
                    TransactionTimeUtc = DateTime.UtcNow
                });

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Withdraw(int id)
        {
            return View(
                new DepositViewModel
                {
                    AccountNumber = id,
                    Account = await _context.Accounts.FindAsync(id)
                });
        }

        [HttpPost]
        public async Task<IActionResult> Withdraw(DepositViewModel viewModel)
        {
            viewModel.Account = await _context.Accounts.FindAsync(viewModel.AccountNumber);

            if (viewModel.Amount > viewModel.Account.Balance)
            {
                ModelState.AddModelError(nameof(viewModel.Amount), "Insufficient funds in account.");
                return View(viewModel);
            }
            if (!verifyAmount(viewModel.Amount))
            {
                return View(viewModel);
            }

            // Note this code could be moved out of the controller, e.g., into the model or repository (design pattern).
            viewModel.Account.Balance -= viewModel.Amount;
            viewModel.Account.Transactions.Add(
                new Transaction
                {
                    TransactionType = (char)TransactionType.Withdraw,
                    Amount = viewModel.Amount,
                    Comment = viewModel.Comment,
                    TransactionTimeUtc = DateTime.UtcNow
                });

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
