using MCBA_Models.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MCBA_Web.ViewModels
{
    public class BillPayViewModel
    {
        public Customer Customer { get; set; }
        public Account Account { get; set; }
        public int? AccountNumber { get; set; }
        public List<int> Accounts { get; set; }
        public List<BillPay> BillPays { get; set; }
        public int PayeeID { get; set; }
        [DataType(DataType.Currency)]
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999999999999999.99)]
        public decimal Amount { get; set; }
        public char Period { get; set; }
        [Display(Name = "Scheduled Time")]
        public DateTime ScheduleTimeUtc { get; set; }
        public string Status { get; set; }
        [Display(Name = "BPay ID")]
        public int BillPayID { get; set; }
    }
}