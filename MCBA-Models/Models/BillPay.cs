using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MCBA_Models.Models;
namespace MCBA_Models.Models
{
    public class BillPay
    {
        [Display(Name ="BillPay ID")]
        public int BillPayID { get; set; }
        [Required, Display(Name ="Account Number")]
        public int AccountNumber { get; set; }
        [Required]
        [Display(Name ="Payee ID")]
        public int PayeeID { get; set; }
        [Required, Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
        [Required]
        public char Period { get; set; }
        [Required, Display(Name = "Scheduled Time")]
        public DateTime ScheduleTimeUtc { get; set; }
        [Required, Display(Name = "Paid Status")]
        public string Status { get; set; }
    }
}
