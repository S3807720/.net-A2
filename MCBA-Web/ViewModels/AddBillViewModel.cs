using MCBA_Models.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace MCBA_Web.ViewModels
{
    public class AddBillViewModel
    {
        public int AccountNumber { get; set; }
        public int? PayeeID { get; set; }
        [DataType(DataType.Currency)]
        [Range(0, 9999999999999999.99)]
        public decimal Amount { get; set; }
        [Required]
        public Period Period { get; set; } = Period.OneOff;
        [Display(Name ="Scheduled Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy:HH}")]
        public DateTime ScheduleTimeUtc { get; set; }
    }
}