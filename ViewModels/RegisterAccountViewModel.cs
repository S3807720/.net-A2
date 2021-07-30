using MCBA_Web.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MCBA_Web.ViewModels
{
    public class RegisterAccountViewModel
    {

        [Display(Name = "Account Number")]
        [RegularExpression(@"^(\d{4})$", ErrorMessage = "Account Number must be 4 digits.")]
        public int AccountNumber { get; set; }

        [Required, StringLength(1)]
        [Display(Name = "Type")]
        public AccountType AccountType { get; set; }

        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }
    }
}