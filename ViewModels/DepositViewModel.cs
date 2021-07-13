using MCBA_Web.Models;

namespace MCBA_Web.ViewModels
{
    public class DepositViewModel
    {
        public int AccountNumber { get; set; }
        public Account Account { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
    }
}
