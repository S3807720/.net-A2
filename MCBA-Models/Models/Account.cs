using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MCBA_Models.Models
{
    public class Account
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Account Number")]
        [RegularExpression(@"^(\d{4})$", ErrorMessage = "Account Number must be 4 digits.")]
        public int AccountNumber { get; set; }

        [Required, StringLength(1)]
        [Display(Name = "Type")]
        public AccountType AccountType { get; set; }

        [StringLength(4), Display(Name = "Customer ID")]
        [ForeignKey("Customer")]
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }

        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }

        public virtual List<Transaction> Transactions { get; set; }
    }
}
