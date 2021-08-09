using Microsoft.AspNetCore.Mvc;
using MCBA_Models.Models;
using System;
using System.Threading.Tasks;
using MCBA_Web.Filters;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MCBA_Web.Graph
{
    [AuthorizeCustomer]
    [ApiController]
    [Route("/[controller]")]
    public class DataController : ControllerBase
    {

        private readonly IHttpClientFactory _clientFactory;
        private HttpClient Client => _clientFactory.CreateClient("api");

        public DataController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        private async Task<List<Transaction>> GetTransactions()
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

        [HttpGet("[action]")]
        public async Task<TransactionCount[]> TransactionsPerMonth()
        {
            var trans = await GetTransactions();
            string[] months = { "January", "February", "March", "April", "May", "June", "July",
                "August", "September", "October", "November", "December" };
            int[] count = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var data = new TransactionCount[months.Length];
            foreach (Transaction ts in trans)
            {
                for (int i = 0; months.Length > i; i++)
                {
                    if (ts.TransactionTimeUtc.ToString("MMMM") == months[i])
                    {
                        count[i]++;
                    }
                }
            }

            for (int i = 0; 12 > i; i++)
            {
                data[i] = new TransactionCount();
                data[i].Month = months[i];
                data[i].Count = count[i];
            }
            return data;
        }


        [HttpGet("[action]")]
        public async Task<TransactionCount[]> TransactionsPerType()
        {
            var trans = await GetTransactions();
            string[] types = { "Withdraw", "Deposit", "Transfer", "BillPay", "Service Fee" };
            int[] count = { 0, 0, 0, 0, 0 };
            var data = new TransactionCount[types.Length];
            foreach (Transaction ts in trans)
            {
                for (int i = 0; types.Length > i; i++)
                {
                    if (types[i].StartsWith(ts.TransactionType))
                    {
                        count[i]++;
                    }
                }
            }

            for (int i = 0; types.Length > i; i++)
            {
                data[i] = new TransactionCount();
                data[i].Month = types[i];
                data[i].Count = count[i];
            }
            return data;
        }


    }
}
