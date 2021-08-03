using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MCBA_Web.Data;
using MCBA_Models.Models;
using MCBA_Web.Utilities;
using MCBA_Web.Filters;
using MCBA_Web.ViewModels;
using Newtonsoft.Json;
using System.Linq;
using X.PagedList;
using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

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
            BillPayViewModel vm = new BillPayViewModel();
            vm.Customer = await _context.Customers.FindAsync(CustomerID);
            vm.BillPays = (List<BillPay>)_context.BillPays.AsQueryable().OrderBy(x => x.AccountNumber).ToList();
            vm.Accounts = vm.Customer.Accounts.Select(x => x.AccountNumber).ToList();
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Index(int AccountNumber)
        {
            BillPayViewModel vm = new BillPayViewModel();
            using (_context)
            {
                vm.Customer = await _context.Customers.FindAsync(CustomerID);
                vm.Accounts = vm.Customer.Accounts.Select(x => x.AccountNumber).ToList();
                vm.BillPays = _context.BillPays.AsQueryable().ToList();
                if (AccountNumber != 0)
                {
                    vm.BillPays.RemoveAll(x => x.AccountNumber != AccountNumber);
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
                bp.Status = ConstantVals.Finished;
            }
            HttpContext.Session.SetInt32("b", id);
            return View(bp);
        }

        [HttpPost]
        public async Task<IActionResult> Confirm()
        {
            int id = (int)HttpContext.Session.GetInt32("b");
            Console.Error.WriteLine(id);
            BillPay bp = await _context.BillPays.FindAsync(id);
            bp.Status = ConstantVals.Finished;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }


}
