using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using MCBA_Models.Models;
using MCBA_Models.Utilities;

namespace MCBA_Web.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<McbaContext>();

            // Look for customers.
            if(context.Customers.Any())
                return; // DB has already been seeded.

            context.Customers.AddRange(
                new Customer
                {
                    CustomerID = 2100,
                    Name = "Matthew Bolger",
                    Address = "123 Fake Street",
                    Suburb = "Melbourne",
                    PostCode = "3000"
                },
                new Customer
                {
                    CustomerID = 2200,
                    Name = "Rodney Cocker",
                    Address = "456 Real Road",
                    Suburb = "Melbourne",
                    PostCode = "3005"
                },
                new Customer
                {
                    CustomerID = 2300,
                    Name = "Shekhar Kalra"
                });

            context.Logins.AddRange(
                new Login
                {
                    LoginID = 12345678,
                    CustomerID = 2100,
                    PasswordHash = "YBNbEL4Lk8yMEWxiKkGBeoILHTU7WZ9n8jJSy8TNx0DAzNEFVsIVNRktiQV+I8d2"
                },
                new Login
                {
                    LoginID = 38074569,
                    CustomerID = 2200,
                    PasswordHash = "EehwB3qMkWImf/fQPlhcka6pBMZBLlPWyiDW6NLkAh4ZFu2KNDQKONxElNsg7V04"
                },
                new Login
                {
                    LoginID = 17963428,
                    CustomerID = 2300,
                    PasswordHash = "LuiVJWbY4A3y1SilhMU5P00K54cGEvClx5Y+xWHq7VpyIUe5fe7m+WeI0iwid7GE"
                });

            context.Accounts.AddRange(
                new Account
                {
                    AccountNumber = 4100,
                    AccountType = AccountType.Savings,
                    CustomerID = 2100,
                    Balance = 100
                },
                new Account
                {
                    AccountNumber = 4101,
                    AccountType = AccountType.Checking,
                    CustomerID = 2100,
                    Balance = 500
                },
                new Account
                {
                    AccountNumber = 4200,
                    AccountType = AccountType.Savings,
                    CustomerID = 2200,
                    Balance = 500.95m
                },
                new Account
                {
                    AccountNumber = 4300,
                    AccountType = AccountType.Checking,
                    CustomerID = 2300,
                    Balance = 1250.50m
                });
            
            const string format = "dd/MM/yyyy hh:mm:ss tt";

            context.Transactions.AddRange(
                new Transaction
                {
                    TransactionType = (char)TransactionType.Deposit,
                    AccountNumber = 4100,
                    Amount = 100,
                    Comment = "Opening balance",
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 08:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = (char)TransactionType.Deposit,
                    AccountNumber = 4101,
                    Amount = 200,
                    Comment = "First deposit",
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 08:30:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = (char)TransactionType.Deposit,
                    AccountNumber = 4100,
                    Amount = 100,
                    Comment = "Dada balance",
                    TransactionTimeUtc = DateTime.ParseExact("19/02/2021 08:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = (char)TransactionType.Deposit,
                    AccountNumber = 4101,
                    Amount = 250,
                    Comment = "First hel",
                    TransactionTimeUtc = DateTime.ParseExact("19/01/2021 08:30:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = (char)TransactionType.Deposit,
                    AccountNumber = 4101,
                    Amount = 300,
                    Comment = "Second deposit",
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 08:45:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = (char)TransactionType.Deposit,
                    AccountNumber = 4200,
                    Amount = 500,
                    Comment = "Deposited $500",
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 09:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = (char)TransactionType.Deposit,
                    AccountNumber = 4200,
                    Amount = 0.95m,
                    Comment = "Deposited $0.95",
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 09:15:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = (char)TransactionType.Deposit,
                    AccountNumber = 4200,
                    Amount = 0.95m,
                    Comment = "Deposited $0.95",
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 09:15:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = (char)TransactionType.Deposit,
                    AccountNumber = 4100,
                    Amount = 500,
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 08:45:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = (char)TransactionType.Deposit,
                    AccountNumber = 4300,
                    Amount = 500,
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 08:45:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = (char)TransactionType.Deposit,
                    AccountNumber = 4101,
                    Amount = 500,
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 08:45:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = (char)TransactionType.Deposit,
                    AccountNumber = 4200,
                    Amount = 500,
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 08:45:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = (char)TransactionType.Deposit,
                    AccountNumber = 4300,
                    Amount = 1250.50m,
                    Comment = null,
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 10:00:00 PM", format, null)
                });
            context.Payees.AddRange(
                new Payee
                {
                    Name = "Bobby Lee Cars",
                    Address = "145 Fake St",
                    Suburb = "Wheresville",
                    State = "NSW",
                    PostCode = "1234",
                    Phone = "(61) 1234 5678"
                },
                new Payee
                {
                    Name = "Jamie Lee Cats",
                    Address = "145 Thorn St",
                    Suburb = "Whosville",
                    State = "TAS",
                    PostCode = "2345",
                    Phone = "(61) 4333 5678"
                },
                new Payee
                {
                    Name = "Janie Zimil Boats",
                    Address = "75 Decker St",
                    Suburb = "Warsville",
                    State = "WA",
                    PostCode = "2345",
                    Phone = "(61) 6788 1232"
                },
                new Payee
                {
                    Name = "Bobby Lee Cars",
                    Address = "145 Fake St",
                    Suburb = "Wheresville",
                    State = "NSW",
                    PostCode = "1234",
                    Phone = "(61) 1234 5678"
                },
                new Payee
                {
                    Name = "Jame Lee Brooms",
                    Address = "15 St St",
                    Suburb = "Howsville",
                    State = "VIC",
                    PostCode = "3343",
                    Phone = "(31) 4567 1234"
                });
            context.BillPays.AddRange(
                new BillPay
                {
                    AccountNumber = 4100,
                    PayeeID = 1,
                    Amount = 5,
                    ScheduleTimeUtc = DateTime.UtcNow,
                    Period = (char) Period.Monthly,
                    Status = ConstantVals.Failed
                },
                new BillPay
                {
                    AccountNumber = 4101,
                    PayeeID = 1,
                    Amount = 7.5m,
                    ScheduleTimeUtc = DateTime.UtcNow,
                    Period = (char)Period.Monthly,
                    Status = ConstantVals.Failed
                },
                 new BillPay
                 {
                     AccountNumber = 4101,
                     PayeeID = 1,
                     Amount = 73.5m,
                     ScheduleTimeUtc = DateTime.UtcNow,
                     Period = (char)Period.OneOff,
                     Status = ConstantVals.Failed
                 },
                 new BillPay
                 {
                     AccountNumber = 4100,
                     PayeeID = 1,
                     Amount = 753.5m,
                     ScheduleTimeUtc = DateTime.UtcNow,
                     Period = (char)Period.OneOff,
                     Status = ConstantVals.Failed
                 },
                 new BillPay
                 {
                     AccountNumber = 4101,
                     PayeeID = 2,
                     Amount = 45.29m,
                     ScheduleTimeUtc = DateTime.UtcNow,
                     Period = (char)Period.Annually,
                     Status = ConstantVals.Failed
                 },
                 new BillPay
                 {
                     AccountNumber = 4100,
                     PayeeID = 2,
                     Amount = 4.29m,
                     ScheduleTimeUtc = DateTime.UtcNow.AddDays(5),
                     Period = (char)Period.Monthly,
                     Status = ConstantVals.Paid
                 },
                new BillPay
                {
                    AccountNumber = 4100,
                    PayeeID = 2,
                    Amount = 5.2m,
                    ScheduleTimeUtc = DateTime.UtcNow,
                    Period = (char)Period.Monthly,
                    Status = ConstantVals.Paid
                });
            context.SaveChanges();
        }
    }
}
