using MCBA_Web.Data;
using MCBA_Web.Filters;
using MCBA_Models.Models;
using MCBA_Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleHashing;
using System.Threading.Tasks;

namespace MCBA_Web.Controllers
{
    public class ProfileController : Controller
    {
        private const string account = "Account";
        private readonly McbaContext _context;

        // ReSharper disable once PossibleInvalidOperationException
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
        private int LoginID => HttpContext.Session.GetInt32(nameof(Login.LoginID)).Value;
      //  HttpContext.Session.getString(nameof(login.LoginID), login.LoginID);
        public ProfileController(McbaContext context) => _context = context;

        [AuthorizeCustomer]
        public async Task<IActionResult> Index()
        {
            var customer = await _context.Customers.FindAsync(CustomerID);
            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(Customer cust)
        {
            var customer = await _context.Customers.FindAsync(cust.CustomerID);
            customer.Address = cust.Address;
            customer.TFN = cust.TFN;
            customer.State = cust.State;
            customer.Suburb = cust.Suburb;
            customer.PostCode = cust.PostCode;
            customer.Name = cust.Name;
            customer.Mobile = cust.Mobile;
            HttpContext.Session.SetString(nameof(Customer.Name), customer.Name);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ChangePassword()
        {
            var customer = await _context.Customers.FindAsync(CustomerID);
            return View( 
                new PasswordViewModel
                {
                    Name = customer.Name,
                    CustomerID = customer.CustomerID,
                });
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(PasswordViewModel login)
        {
            var log = await _context.Logins.FindAsync(LoginID);
            if (log == null) return RedirectToAction(nameof(Index));
            log.PasswordHash = PBKDF2.Hash(login.Password);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
