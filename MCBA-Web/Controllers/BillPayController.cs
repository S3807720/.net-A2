using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MCBA_Web.Data;
using MCBA_Models.Models;
using MCBA_Web.Filters;
using MCBA_Web.ViewModels;
using System.Linq;
using X.PagedList;
using System.Collections.Generic;
using MCBA_Models.Utilities;

namespace MCBA_Web.Controllers
{
    // Can add authorize attribute to controllers.
    [AuthorizeCustomer]
    public class BillPayController : Controller
    {
        private readonly McbaContext _context;
        // ReSharper disable once PossibleInvalidOperationException
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public BillPayController(McbaContext context) => _context = context;

        // Can add authorize attribute to actions.
        [AuthorizeCustomer]
        public async Task<IActionResult> Index()
        {
            BillPayViewModel vm = new();
            vm.Customer = await _context.Customers.FindAsync(CustomerID);
            vm.BillPays = _context.BillPays.AsQueryable().OrderBy(x => x.AccountNumber).ToList();
            vm.Accounts = vm.Customer.Accounts.Select(x => x.AccountNumber).ToList();
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Index(int AccountNumber)
        {
            HttpContext.Session.SetInt32(nameof(Account.AccountNumber), AccountNumber);
            BillPayViewModel vm = new();
            using (_context)
            {
                vm.Customer = await _context.Customers.FindAsync(CustomerID);
                vm.Accounts = vm.Customer.Accounts.Select(x => x.AccountNumber).ToList();
                vm.BillPays = _context.BillPays.AsQueryable().OrderByDescending(x => x.ScheduleTimeUtc).ToList();
                if (AccountNumber != 0)
                {
                    vm.AccountNumber = AccountNumber;
                    vm.BillPays.RemoveAll(x => x.AccountNumber != AccountNumber);
                }
                else
                {
                    vm.AccountNumber = null;
                }

            }
            return View(vm);
        }

        public async Task<IActionResult> Confirm(int id)
        {
            BillPay bp = null;
            using (_context)
            {
                bp = await _context.BillPays.FindAsync(id);
            }
            HttpContext.Session.SetInt32(nameof(BillPay.BillPayID), id);
            return View(bp);
        }

        [HttpPost]
        public async Task<IActionResult> Confirm()
        {
            int id = (int)HttpContext.Session.GetInt32(nameof(BillPay.BillPayID));
            BillPay bp = await _context.BillPays.FindAsync(id);
            bp.Status = ConstantVals.Finished;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {
            BillPay bp = null;
            using (_context)
            {
                bp = await _context.BillPays.FindAsync(id);
                bp.Status = ConstantVals.Finished;
            }
            HttpContext.Session.SetInt32(nameof(BillPay.BillPayID), id);
            return View(bp);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(BillPay bill)
        {
            if (bill.Amount <= 0 || bill.Amount.HasMoreThanTwoDecimalPlaces())
            {
                ModelState.AddModelError(nameof(bill.Amount), "Amount must be positive and a maximum of 2 decimals.");
                return View(bill);
            }
            if (DateTime.UtcNow > bill.ScheduleTimeUtc)
            {
                ModelState.AddModelError(nameof(bill.ScheduleTimeUtc), "Date can't be set in the past.");
                return View(bill);
            }
            int id = (int)HttpContext.Session.GetInt32(nameof(BillPay.BillPayID));
            BillPay bp = await _context.BillPays.FindAsync(id);
            bp.Status = ConstantVals.Paid;
            bp.Amount = bill.Amount;
            bp.PayeeID = bill.PayeeID;
            bp.ScheduleTimeUtc = bill.ScheduleTimeUtc;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult AddPayment()
        {
            return View(new AddBillViewModel()
            {
                AccountNumber = (int)HttpContext.Session.GetInt32(nameof(Account.AccountNumber))
            });
        }
        [HttpPost]
        public async Task<IActionResult> AddPayment(AddBillViewModel vm)
        {
            vm.AccountNumber = (int)HttpContext.Session.GetInt32(nameof(Account.AccountNumber));
            if (DateTime.UtcNow > vm.ScheduleTimeUtc)
            {
                ModelState.AddModelError(nameof(vm.ScheduleTimeUtc), "Date can't be set in the past.");
                return View(vm);
            }
            if (vm.Amount <= 0 || vm.Amount.HasMoreThanTwoDecimalPlaces())
            {
                ModelState.AddModelError(nameof(vm.Amount), "Amount must be positive and a maximum of 2 decimals.");
                return View(vm);
            }
            if (vm.PayeeID == null)
            {
                ModelState.AddModelError(nameof(vm.PayeeID), "Payee must not be empty.");
                return View(vm);
            }
            BillPay bp = new();
            bp.AccountNumber = vm.AccountNumber;
            bp.Amount = vm.Amount;
            bp.PayeeID = (int)vm.PayeeID;
            bp.Period = (char)vm.Period;
            bp.ScheduleTimeUtc = vm.ScheduleTimeUtc.ToUniversalTime();
            bp.Status = ConstantVals.Paid;
            _context.BillPays.Add(bp);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Pay(int id)
        {
            BillPay bp = await _context.BillPays.FindAsync(id);
            HttpContext.Session.SetInt32(nameof(bp.BillPayID), bp.BillPayID);
            return View(bp);
        }
        [HttpPost]
        public async Task<IActionResult> Pay()
        {
            BillPay bp = await _context.BillPays.FindAsync(HttpContext.Session.GetInt32(nameof(bp.BillPayID))); 
            Account acc = await _context.Accounts.FindAsync(bp.AccountNumber); 
            var val = acc.AccountType == AccountType.Checking ? ConstantVals.Minimum_Checking : ConstantVals.Minimum_Savings;
            if (acc.Balance - val < bp.Amount)
            {
                ModelState.AddModelError(nameof(bp.Amount), "Insufficient funds in the account.");
                return View(bp);
            }

            acc.Transactions.Add(new Transaction
            {
                TransactionType = (char)TransactionType.BillPay,
                Amount = bp.Amount,
                AccountNumber = bp.AccountNumber,
                TransactionTimeUtc = DateTime.UtcNow,
                Comment = String.Format("Payee: {0}", bp.PayeeID)
            });
            if (bp.Period == (char)Period.OneOff)
            {
                bp.Status = ConstantVals.Finished;
            }
            else
            {
                bp.Status = ConstantVals.Paid;
            }

            acc.Balance -= bp.Amount;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }


}
