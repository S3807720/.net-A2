using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MCBA_Web.Models
{
    public class BillPay
    {
        [Display(Name ="Bill Pay ID")]
        public int BillPayID { get; set; }

        [ForeignKey("Account")][Display(Name ="Account Number")]
        public int AccountNumber { get; set; }
        public virtual Account Account { get; set; }

        [ForeignKey("Payee")]
        [Display(Name ="Payee ID")]
        public int PayeeID { get; set; }
        public virtual Account Payee { get; set; }

        [Required, Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Required]
        public char Period { get; set; }
        [Required]
        public DateTime ScheduleTimeUtc { get; set; }
    }
}
