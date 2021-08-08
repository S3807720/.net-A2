using MCBA_Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MCBA_Admin.ViewModels
{
    public class TransactionViewModel
    {
        public List<Account> Accounts { get; set; }
        [Display(Name = "Account Number")]
        public int? AccountNumber { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MMMMM-dd}")]
        [Display(Name ="Start Date")]
        public DateTime? StartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MMMMM-dd}")]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}