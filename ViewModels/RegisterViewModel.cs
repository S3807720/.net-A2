using MCBA_Web.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MCBA_Web.ViewModels
{
    public class RegisterViewModel
    {

        [Required]
        [StringLength(4)]
        public int CustomerID { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        [RegularExpression(@"^(\d{11})$", ErrorMessage = "Tax File Number must be of 11 digits.")]
        public string TFN { get; set; }
        [MaxLength(50)]
        public string Address { get; set; }

        [MaxLength(40)]
        public string Suburb { get; set; }
        [StringLength(3)]
        public string State { get; set; }

        [StringLength(4)]
        public string PostCode { get; set; }

        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Number must be of the format 04xx xxx xxx.")]
        [StringLength(12)]
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }
    }
}