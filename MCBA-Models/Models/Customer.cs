using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MCBA_Models.Models
{
    public class Customer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CustomerID { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [RegularExpression(@"^(\d{11})$", ErrorMessage = "Tax File Number must be of 11 digits.")]
        public string TFN { get; set; }
        [MaxLength(50)]
        public string Address { get; set; }

        [MaxLength(40)]
        public string Suburb { get; set; }
        [MaxLength(3)][MinLength(2)]
        public string State { get; set; }

        [StringLength(4)]
        public string PostCode { get; set; }

        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Number must be of the format 04xx xxx xxx. (Without spaces)")]
        [StringLength(10)]
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }
        public virtual List<Account> Accounts { get; set; }

        public bool CanLogin { get; set; } = true;
    }
}
