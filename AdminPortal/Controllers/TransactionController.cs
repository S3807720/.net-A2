using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MCBA_Web.Filters;
using Newtonsoft.Json;
using MCBA_Models.Models;
using System.Net.Http;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MCBA_Admin.ViewModels;

namespace MCBA_Admin.Controllers
{
    // Can add authorize attribute to controllers.
    [AuthorizeCustomer]
    public class TransactionController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private HttpClient Client => _clientFactory.CreateClient("api");

        public TransactionController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        // Can add authorize attribute to actions.
        [AuthorizeCustomer]
        public async Task<IActionResult> Index()
        {
            var acc = await GetAccs();
            var trans = await GetTransactions(); 
            //customer.Accounts = acc;
            return View(new TransactionViewModel()
            {
                Accounts = acc,
                Transactions = trans
            });
        }

         [HttpPost]
         public async Task<IActionResult> Index(TransactionViewModel vm)
         {
            vm.Transactions = await GetTransactions(vm.AccountNumber, vm.StartDate, vm.EndDate);
            vm.Accounts = await GetAccs();
            
            return View(vm);
         }
        public async Task<List<Transaction>> GetTransactions(int? id, DateTime? start, DateTime? end)
        {
            var trans = await GetTransactions();
            //values can be null(empty), filter the transactions based on the information if it exists, otherwise just show all
            if (id != null)
            {
                trans = trans.Where(x => x.AccountNumber == id).ToList();
            }
            if (start != null)
            {
                trans = trans.Where(x => x.TransactionTimeUtc > start).ToList();
            }
            if (end != null)
            {
                trans = trans.Where(x => x.TransactionTimeUtc < end).ToList();
            }
            return trans;
        }

        public async Task<List<Account>> GetAccs()
        {
            var response = await Client.GetAsync($"api/account");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            var result = await response.Content.ReadAsStringAsync();
            var acc = JsonConvert.DeserializeObject<List<Account>>(result);
            return acc;
        }

        public async Task<List<Transaction>> GetTransactions()
        {
            var response = await Client.GetAsync($"api/transaction");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            var result = await response.Content.ReadAsStringAsync();
            var trans = JsonConvert.DeserializeObject<List<Transaction>>(result);
            return trans;
        }

    }


}
