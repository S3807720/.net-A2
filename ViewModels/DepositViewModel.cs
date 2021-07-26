using MCBA_Web.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MCBA_Web.ViewModels
{
    public class DepositViewModel
    {
        public int AccountNumber { get; set; }
        [JsonIgnore]
        public Account Account { get; set; }
        [Display(Name = "Destination Account Number")]
        public int? DestinationAccNumber { get; set; }
        public char Type { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
    }
}