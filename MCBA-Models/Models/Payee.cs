using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MCBA_Models.Models
{
    public class Payee
    {
        public int PayeeID { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(50)]
        public string Address { get; set; }

        [Required, MaxLength(40)]
        public string Suburb { get; set; }
        [MaxLength(3)][MinLength(2)]
        public string State { get; set; }

        [Required, StringLength(4)]
        public string PostCode { get; set; }

        [RegularExpression(@"(\()(0\d{1})(\))^(\d{8})$", ErrorMessage = "Number must be of the format (0x) xxxx xxxx. (Without spaces)")]
        [StringLength(14)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
    }
}
