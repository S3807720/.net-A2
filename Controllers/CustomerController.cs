using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MCBA_Web.Data;
using MCBA_Web.Models;
using MCBA_Web.Utilities;
using MCBA_Web.Filters;
using MCBA_Web.ViewModels;
using Newtonsoft.Json;
using System.Web.Providers.Entities;

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

            verifyAmount(viewModel.Amount);
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            // Note this code could be moved out of the controller, e.g., into the model or repository (design pattern).
            viewModel.Account.Balance += viewModel.Amount;
            var trans =
                new Transaction
                {
                    TransactionType = (char)TransactionType.Deposit,
                    Amount = viewModel.Amount,
                    Comment = viewModel.Comment,
                    TransactionTimeUtc = DateTime.UtcNow
                };
            viewModel.Account.Transactions.Add(trans);

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
            }
            verifyAmount(viewModel.Amount);
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            // Note this code could be moved out of the controller, e.g., into the model or repository (design pattern).
            viewModel.Account.Balance -= viewModel.Amount;
            var trans =
                new Transaction
                {
                    TransactionType = (char)TransactionType.Withdraw,
                    Amount = viewModel.Amount,
                    Comment = viewModel.Comment,
                    TransactionTimeUtc = DateTime.UtcNow
                };
            viewModel.Account.Transactions.Add(trans);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Transfer(int id)
        {
            return View(
                new DepositViewModel
                {
                    AccountNumber = id,
                    Account = await _context.Accounts.FindAsync(id)
                });
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(DepositViewModel viewModel)
        {

            viewModel.Account = await _context.Accounts.FindAsync(viewModel.AccountNumber);

            if (viewModel.Amount > viewModel.Account.Balance)
            {
                ModelState.AddModelError(nameof(viewModel.DestinationAccNumber), "Insufficient funds in account.");
            }
            if (viewModel.AccountNumber == viewModel.DestinationAccNumber)
            {
                ModelState.AddModelError(nameof(viewModel.DestinationAccNumber), "You cannot transfer money to the same account.");
            }
            if (_context.Accounts.Find(viewModel.DestinationAccNumber) == null)
            {
                ModelState.AddModelError(nameof(viewModel.DestinationAccNumber), "Destination account does not exist.");
            }
            verifyAmount(viewModel.Amount);
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            viewModel.Type = (char)TransactionType.Transfer;
            //var trans =
            //    new Transaction
            //    {
            //        TransactionType = (char)TransactionType.Transfer,
            //        DestinationAccountNumber = viewModel.DestinationAccNumber,
            //        Amount = viewModel.Amount,
            //        Comment = viewModel.Comment,
            //        TransactionTimeUtc = DateTime.UtcNow
            //    };
            //HttpContext.Session.SetObject("transaction", trans);
            // viewModel.Account.Transactions.Add(trans);
            HttpContext.Session.SetObject("DepositViewModel", viewModel);
            HttpContext.Session.SetString("type", "Transfer");
            return RedirectToAction(nameof(Confirm));
        }
        public async Task addTransferTransaction(DepositViewModel viewModel)
        {
            Account dest = _context.Accounts.Find(viewModel.DestinationAccNumber);
            dest.Transactions.Add(
                new Transaction
                {
                    TransactionType = (char)viewModel.Type,
                    Amount = viewModel.Amount,
                    Comment = viewModel.Comment,
                    TransactionTimeUtc = DateTime.UtcNow
                });
            dest.Balance += viewModel.Amount;

            // Note this code could be moved out of the controller, e.g., into the model or repository (design pattern).

            await _context.SaveChangesAsync();
        }

        public async Task addTransaction(DepositViewModel viewModel, char type)
        {
            var acc = await _context.Accounts.FindAsync(viewModel.AccountNumber);
            var trans =
                new Transaction
                {
                    TransactionType = type,
                    Amount = viewModel.Amount,
                    DestinationAccountNumber = viewModel.DestinationAccNumber,
                    Comment = viewModel.Comment,
                    TransactionTimeUtc = DateTime.UtcNow
                };
            acc.Transactions.Add(trans);
            _ = viewModel.Type == 'D' ? acc.Balance += viewModel.Amount : acc.Balance -= viewModel.Amount;
        }
        public async Task<IActionResult> Confirm(int id)
        {
            DepositViewModel dvm = HttpContext.Session.GetObject<DepositViewModel>("depositViewModel");
            dvm.Account = await _context.Accounts.FindAsync(dvm.AccountNumber);
            return View(dvm);
        }
        [HttpPost]
        public async Task<IActionResult> Confirm()
        {
            DepositViewModel viewModel = HttpContext.Session.GetObject<DepositViewModel>("DepositViewModel");

            char type = HttpContext.Session.GetString("type")[0];
            await addTransaction(viewModel, type);
            if (type == (char)TransactionType.Transfer)
            {
                await addTransferTransaction(viewModel);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }


}
