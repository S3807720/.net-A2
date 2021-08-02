using MCBA_Models.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MCBA_Web.ViewModels
{
    public class BillPayViewModel
    {
        public Customer Customer { get; set; }
        public Account Account { get; set; }
        public List<Account> Accounts { get; set; }
        public List<BillPay> BillPays { get; set; }
    }
}