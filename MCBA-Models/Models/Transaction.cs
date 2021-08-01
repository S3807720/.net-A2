using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MCBA_Models.Models
{
    public record Transaction
    {
        [Display(Name ="Transaction ID")]
        public int TransactionID { get; set; }
        [Column(TypeName="nvarchar(1)")]
        [Display(Name = "Transaction Type")]
        public char TransactionType { get; set; }

        [ForeignKey("Account")]
        public int AccountNumber { get; set; }
        public virtual Account Account { get; set; }

        [ForeignKey("DestinationAccount")]
        [Display(Name = "Destination Account")]
        public int? DestinationAccountNumber { get; set; }
        public virtual Account DestinationAccount { get; set; }

        [Required, Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [StringLength(30)]
        public string Comment { get; set; }
        [Required]
        [Display(Name = "Transaction Date")]
        public DateTime TransactionTimeUtc { get; set; }
    }
}
