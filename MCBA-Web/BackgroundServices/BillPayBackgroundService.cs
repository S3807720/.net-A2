using System;
using System.Threading;
using System.Threading.Tasks;
using MCBA_Web.Data;
using MCBA_Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Mcba_Web.BackgroundServices
{
    public class BillPayBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<BillPayBackgroundService> _logger;

        public BillPayBackgroundService(IServiceProvider services, ILogger<BillPayBackgroundService> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("People Background Service is running.");

            while (!cancellationToken.IsCancellationRequested)
            {
                await DoWork(cancellationToken);

                _logger.LogInformation("Checking due payments in 5 minutes.");

                await Task.Delay(TimeSpan.FromMinutes(5), cancellationToken);
            }
        }

        private async Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Running billpay payments.");

            using var scope = _services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<McbaContext>();

            var bpays = await context.BillPays.ToListAsync(cancellationToken);
            foreach (var bpay in bpays)
            {
                if (bpay.Status != ConstantVals.Finished && bpay.Status != ConstantVals.Blocked)
                {
                    if (DateTime.UtcNow.ToFileTimeUtc() > bpay.ScheduleTimeUtc.ToFileTimeUtc())
                    {
                        Account acc = await context.Accounts.FindAsync(bpay.AccountNumber);
                        var val = acc.AccountType == AccountType.Checking ? ConstantVals.Minimum_Checking : ConstantVals.Minimum_Savings;
                        if (acc.Balance - val > bpay.Amount)
                        {
                            AddTransaction(context, bpay);
                        }
                        else
                        {
                            bpay.Status = ConstantVals.Failed;
                        }

                    }
                }

            }

            await context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("BillPay Service work complete.");
        }
        //do the legwork of the transactions
        //add transaction with bp info
        //change the next payment to be whenever it's set
        //add balance etc
        private async void AddTransaction(McbaContext context, BillPay bp)
        {
            Account acc = await context.Accounts.FindAsync(bp.AccountNumber);
            acc.Transactions.Add(new Transaction
            {
                TransactionType = (char)TransactionType.BillPay,
                Amount = bp.Amount,
                AccountNumber = bp.AccountNumber,
                TransactionTimeUtc = DateTime.UtcNow,
                Comment = String.Format("Payee: {0}", bp.PayeeID)
            });
            bp.Status = ConstantVals.Paid;
            if (bp.Period == (char)Period.Monthly)
            {
                bp.ScheduleTimeUtc = bp.ScheduleTimeUtc.AddMonths(1);
            } else if (bp.Period == (char) Period.Annually)
            {
                bp.ScheduleTimeUtc = bp.ScheduleTimeUtc.AddYears(1);
            } else if (bp.Period == (char) Period.Quarterly)
            {
                bp.ScheduleTimeUtc = bp.ScheduleTimeUtc.AddMonths(3);
            } else
            {
                bp.Status = ConstantVals.Finished;
            }
            
            acc.Balance -= bp.Amount;
        }
    }
}
