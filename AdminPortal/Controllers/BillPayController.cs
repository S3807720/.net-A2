using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MCBA_Models.Models;
using MCBA_Web.Filters;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Json;
using MCBA_Models.Utilities;
using System.Linq;

namespace MCBA_Admin.Controllers
{
    // Can add authorize attribute to controllers.
    [AuthorizeCustomer]
    public class BillPayController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private HttpClient Client => _clientFactory.CreateClient("api");

        public BillPayController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        // Can add authorize attribute to actions.
        [AuthorizeCustomer]
        public async Task<IActionResult> Index()
        {
            var bills = await GetBills();
            return View(bills);
        }
        public async Task<List<BillPay>> GetBills()
        {
            var response = await Client.GetAsync($"api/billpay");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            var result = await response.Content.ReadAsStringAsync();
            var bills = JsonConvert.DeserializeObject<List<BillPay>>(result).OrderByDescending(x => x.ScheduleTimeUtc).ToList();
            //bills.OrderByDescending(x => x.ScheduleTimeUtc);
            return bills;
        }
        public async Task<IActionResult> Block(int id)
        {
            var response = await Client.GetAsync($"api/billpay/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            var result = await response.Content.ReadAsStringAsync();
            var bp = JsonConvert.DeserializeObject<BillPay>(result);
            bp.Status = bp.Status == ConstantVals.Blocked ? ConstantVals.Paid : ConstantVals.Blocked;
            var content = JsonContent.Create(bp);
            response = Client.PutAsync("api/billpay", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "BillPay", new { area = "" });
            }
            var bills = await GetBills();
            return View(bills);
        }
    }
}
