using MCBA_Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MCBA_Web.ViewModels
{
    public class BillPayViewModel
    {
        public Customer Customer { get; set; }
        public Account Account { get; set; }
        public int AccountNumber { get; set; }
        public List<int> Accounts { get; set; }
        public List<BillPay> BillPays { get; set; }
        [Display(Name = "Payee ID")]
        public int PayeeID { get; set; }
        [Required, Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
        [Required]
        public char Period { get; set; }
        [Required, Display(Name = "Scheduled Time")]
        public DateTime ScheduleTimeUtc { get; set; }
        public string Status { get; set; }
        public int BillPayID { get; set; }
    }
}