﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MCBA_Web.Data;
using MCBA_Web.Models;
using MCBA_Web.Utilities;
using MCBA_Web.Filters;
using MCBA_Web.ViewModels;
using Newtonsoft.Json;
using System.Linq;
using X.PagedList;

namespace MCBA_Web.Controllers
{
    // Can add authorize attribute to controllers.
    [AuthorizeCustomer]
    public class CustomerController : Controller
    {
        private const string account = "Account";
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

        public async Task<IActionResult> Deposit(int id)
        {
            return View(
                new DepositViewModel
                {
                    AccountNumber = id,
                    Account = await _context.Accounts.FindAsync(id)
                });
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
            viewModel.Type = (char)TransactionType.Deposit;
            HttpContext.Session.SetObject(account, viewModel);
            return RedirectToAction(nameof(Confirm));
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
            viewModel.Type = (char)TransactionType.Withdraw;
            HttpContext.Session.SetObject(account, viewModel);
            return RedirectToAction(nameof(Confirm));
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
            HttpContext.Session.SetObject(account, viewModel);
            return RedirectToAction(nameof(Confirm));
        }

        public async Task addTransaction(DepositViewModel viewModel)
        {
            var acc = await _context.Accounts.FindAsync(viewModel.AccountNumber);
            var trans = new Transaction
            {
                TransactionType = viewModel.Type,
                Amount = viewModel.Amount,
                DestinationAccountNumber = viewModel.DestinationAccNumber,
                Comment = viewModel.Comment,
                TransactionTimeUtc = DateTime.UtcNow
            };
            acc.Transactions.Add(trans);
            if (viewModel.Type == 'D')
                acc.Balance += viewModel.Amount;
            else {
                if (viewModel.Type == 'T')
                {
                    var dest = await _context.Accounts.FindAsync(viewModel.DestinationAccNumber);
                    dest.Transactions.Add(trans with {
                        DestinationAccountNumber = null,
                        AccountNumber = dest.AccountNumber});
                    dest.Balance += trans.Amount;
                } 
                acc.Balance -= trans.Amount;
            }
        }
        public async Task<IActionResult> Confirm(int id)
        {
            DepositViewModel dvm = HttpContext.Session.GetObject<DepositViewModel>(account);
            dvm.Account = await _context.Accounts.FindAsync(dvm.AccountNumber);
            return View(dvm);
        }
        [HttpPost]
        public async Task<IActionResult> Confirm()
        {
            DepositViewModel viewModel = HttpContext.Session.GetObject<DepositViewModel>(account);
            await addTransaction(viewModel);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ViewStatements(Account acc)
        {
           // var customer = acc;
            if (acc == null)
                return NotFound();

            // Store a complex object in the session via JSON serialisation.
            var accountJson = JsonConvert.SerializeObject(acc);
           // HttpContext.Session.SetString(account, accountJson);
            HttpContext.Session.SetObject(account, acc);
            return RedirectToAction(nameof(Statements));
        }

        public async Task<IActionResult> Statements(int page = 1)
        {
            var acc = HttpContext.Session.GetObject<Account>(account);
            if (acc == null)
                return RedirectToAction(nameof(Index)); // OR return BadRequest();

            // Retrieve complex object from the session via JSON deserialisation.
           // var acc = JsonConvert.DeserializeObject<Transaction>(accountJson);
            ViewBag.Account = acc;
            // Page the orders, maximum of 3 per page.
            const int pageSize = 4;
            var pagedList = await _context.Transactions.Where(x => x.AccountNumber == acc.AccountNumber).
                 OrderBy(x => x.TransactionTimeUtc).ToPagedListAsync(page, pageSize);
            //var pagedList = await acc.Transactions.OrderBy(x => x.TransactionTimeUtc).ToPagedListAsync((int)page, pageSize);
            return View(pagedList);
        }

    }


}