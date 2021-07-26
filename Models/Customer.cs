using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MCBA_Web.Models
{
    public class Customer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CustomerID { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        [RegularExpression(@"^(\d{11})$", ErrorMessage = "Tax File Number must be of 11 digits.")]
        public string TFN { get; set; }
        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(40)]
        public string Suburb { get; set; }
        [StringLength(3)]
        public string State { get; set; }

        [StringLength(4)]
        public string PostCode { get; set; }

        [RegularExpression(@"^(\d{12})$", ErrorMessage = "Number must be of the format 04xx xxx xxx.")]
        [StringLength(12)]
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }
        public virtual List<Account> Accounts { get; set; }
    }
}
