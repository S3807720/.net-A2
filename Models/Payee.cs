﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MCBA_Web.Models
{
    public class Payee
    {
        public int PayeeID { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required, StringLength(50)]
        public string Address { get; set; }

        [Required, StringLength(40)]
        public string Suburb { get; set; }
        [StringLength(3)]
        public string State { get; set; }

        [Required, StringLength(4)]
        public string PostCode { get; set; }

        [Required, RegularExpression(@"^(\d{12})$", ErrorMessage = "Number must be of the format 04xx xxxx xxxx.")]
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }
    }
}
