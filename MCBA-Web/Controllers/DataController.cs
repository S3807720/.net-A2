using Microsoft.AspNetCore.Mvc;
using MCBA_Models.Models;
using MCBA_Web.Data;
using System.Linq;
using System;
using MCBA_Web.Controllers;
using MCBA_Models.Utilities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MCBA_Web.Filters;

namespace MCBA_Web.Graph
{
    [AuthorizeCustomer]
    [ApiController]
    [Route("/[controller]")]
    public class DataController : ControllerBase
    {

        private readonly McbaContext _context;

        public DataController(McbaContext context) => _context = context;

        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;


        [HttpGet("[action]")]
        public TransactionCount[] TransactionsPerMonth()
        {
            var trans = _context.Transactions.ToList();
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
        public TransactionCount[] TransactionsPerMonthByAccount()
        {
            var accounts = _context.Accounts.Where(x => x.CustomerID == CustomerID).ToList();
            var trans = _context.Transactions.ToList();
            string[] months = { "January", "February", "March", "April", "May", "June", "July",
                "August", "September", "October", "November", "December" };
            int[] count = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var data = new TransactionCount[months.Length];
            foreach (Transaction ts in trans)
            {
                for (int i = 0; months.Length > i; i++)
                {
                    foreach (Account a in accounts)
                    {
                        if (ts.AccountNumber == a.AccountNumber)
                        {
                            if (ts.TransactionTimeUtc.ToString("MMMM") == months[i])
                            {
                                count[i]++;
                            }
                        }
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
        public TransactionCount[] TransactionsPerType()
        {
            var trans = _context.Transactions.ToList();
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

        [HttpGet("[action]")]
        public TransactionCount[] TransactionsPerTypeByAccount()
        {
            var accounts = _context.Accounts.Where(x => x.CustomerID == CustomerID).ToList();
            var trans = _context.Transactions.ToList();
            string[] types = { "Withdraw", "Deposit", "Transfer", "BillPay", "Service Fee" };
            int[] count = { 0, 0, 0, 0, 0 };
            var data = new TransactionCount[types.Length];
            foreach (Transaction ts in trans)
            {
                for (int i = 0; types.Length > i; i++)
                {
                    foreach (Account a in accounts)
                    {
                        if (ts.AccountNumber == a.AccountNumber)
                        {
                            if (types[i].StartsWith(ts.TransactionType))
                            {
                                count[i]++;
                            }
                        }
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

        //generate a bunch of transactions for data/
        //not gonna bother explaining this in detail... 
        //basically a lot of for loops with random numbers to 
        //randomize the amount of each category to give realistic
        //fake data :)
        //using the fee check to add appropriate fee transactions too
        [HttpGet("[action]")]
        public async Task<string> AddTransactions()
        {
            Console.Error.WriteLine("Writing transactions..");
            var accs = _context.Accounts.ToList();
            Transaction ts = new Transaction();
            RandomDateTime date = new RandomDateTime();
            Random rand = new Random();
            ts.Amount = 0.1m; ts.Comment = "For science!";
            foreach (Account ac in accs)
            {
                for (int i = 0; 5 > i; i++)
                {
                    for (int b = 0; 3 > b; b++)
                    {
                        for (int c = 0; rand.Next(1, 5) > c; c++)
                        {
                            ac.Transactions.Add(ts with
                            {
                                TransactionType = (char)TransactionType.Deposit,
                                TransactionTimeUtc = date.Next(),
                            });
                        }
                        for (int c = 0; rand.Next(1, 5) > c; c++)
                        {
                            DateTime time = date.Next();
                            ac.Transactions.Add(ts with
                            {
                                TransactionType = (char)TransactionType.Withdraw,
                                TransactionTimeUtc = time,
                            });
                            if (CustomerController.FeeOrNot(ac))
                            {
                                ac.Transactions.Add(new Transaction
                                {
                                    TransactionType = (char)TransactionType.ServiceCharge,
                                    Amount = ConstantVals.Withdraw_Fee,
                                    TransactionTimeUtc = time,
                                    Comment = String.Format("Transaction Fee of {0}", ConstantVals.Withdraw_Fee)
                                });
                            }
                        }
                        for (int c = 0; rand.Next(1, 5) > c; c++)
                        {
                            DateTime time = date.Next();
                            ac.Transactions.Add(ts with
                            {
                                TransactionType = (char)TransactionType.Transfer,
                                TransactionTimeUtc = time
                            });
                           
                        }


                    }
                }
            }
            await _context.SaveChangesAsync();
            return "Success";

        }

    }
}
