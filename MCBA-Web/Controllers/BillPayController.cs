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
        private const string account = "Account";
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
            vm.BillPays = (List<BillPay>)_context.BillPays.AsQueryable().ToList();
            vm.Accounts = vm.Customer.Accounts;
            return View(vm);
        }
        [HttpPost]
        public IActionResult Index(BillPayViewModel vm, int id)
        {
            using (_context)
            {
                vm.BillPays = (List<BillPay>)_context.BillPays.AsQueryable().ToList().Where(x => x.AccountNumber == id);
            }
            return View(vm);
        }
        [HttpPost]
        public IActionResult Index(BillPayViewModel vm)
        {
            using (_context)
            {
                vm.BillPays = (List<BillPay>)_context.BillPays.AsQueryable().ToList().Where(x => x.AccountNumber == vm.Account.AccountNumber);
            }
            return View(vm);
        }
    }


}
