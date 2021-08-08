using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MCBA_Web.Filters;
using Newtonsoft.Json;
using MCBA_Models.Models;
using System.Net.Http;
using System.Collections.Generic;
using System.Text;
using System.Net.Http.Json;
using MCBA_Models.Utilities;
using System.Linq;
using X.PagedList;

namespace MCBA_Admin.Controllers
{
    // Can add authorize attribute to controllers.
    [AuthorizeCustomer]
    public class CustomerController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private HttpClient Client => _clientFactory.CreateClient("api");

        // ReSharper disable once PossibleInvalidOperationException
        private string CustomerID => HttpContext.Session.GetString(nameof(Customer.CustomerID));

        public CustomerController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        // Can add authorize attribute to actions.
        [AuthorizeCustomer]
        public async Task<IActionResult> Index()
        {
            var customers = await GetCusts();
            //customer.Accounts = acc;
            return View(customers);
        }
        public async Task<List<Customer>> GetCusts()
        {
            var response = await Client.GetAsync($"api/customer");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            var result = await response.Content.ReadAsStringAsync();
            var customers = JsonConvert.DeserializeObject<List<Customer>>(result);
            return customers;
        }
        public async Task<IActionResult> Lock(int id)
        {
            var response = await Client.GetAsync($"api/customer/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            var result = await response.Content.ReadAsStringAsync();
            var customer = JsonConvert.DeserializeObject<Customer>(result);
            customer.CanLogin = customer.CanLogin == false ? true : false;
            var content = JsonContent.Create(customer);
            response = Client.PutAsync("api/customer", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Customer", new { area = "" });
            }
            var customers = await GetCusts();
            return View(customers);
        }


        public async Task<IActionResult> ViewStatementsAsync()
        {
            var response = await Client.GetAsync($"api/transaction");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            var result = await response.Content.ReadAsStringAsync();
            var trans = JsonConvert.DeserializeObject<List<Transaction>>(result);
            return View(trans);
        }

         [HttpPost]
         public async Task<IActionResult> ViewStatementsAsync(int accNum)
         {
            var response = await Client.GetAsync($"api/account/{accNum}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }
            var result = await response.Content.ReadAsStringAsync();
            var acc = JsonConvert.DeserializeObject<Account>(result);
            var trans = acc.Transactions;
            HttpContext.Session.SetObject(nameof(Account), acc);
            return View(trans);
         }

         public async Task<IActionResult> Statements(int page = 1)
         {
             Account acc = HttpContext.Session.GetObject<Account>(nameof(Account));
             if (acc == null)
                 return RedirectToAction(nameof(Index)); // OR return BadRequest();

             // Retrieve complex object from the session via JSON deserialisation.
            // var acc = JsonConvert.DeserializeObject<Transaction>(accountJson);
             ViewBag.Account = acc;
             // Page the orders, maximum of 3 per page.
             const int pageSize = 4;

             var response = await Client.GetAsync($"api/transaction/{acc.AccountNumber}");
             if (!response.IsSuccessStatusCode)
             {
                 return null;
             }

             var result = await response.Content.ReadAsStringAsync();
             var transactions = JsonConvert.DeserializeObject<List<Transaction>>(result);

             var pagedList = await transactions.Where(x => x.AccountNumber == acc.AccountNumber).
                  OrderBy(x => x.TransactionTimeUtc).ToPagedListAsync(page, pageSize);
             //var pagedList = await acc.Transactions.OrderBy(x => x.TransactionTimeUtc).ToPagedListAsync((int)page, pageSize);
             return View(pagedList);
         }



    }


}
