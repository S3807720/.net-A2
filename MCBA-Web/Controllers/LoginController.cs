using System.Threading.Tasks;
using MCBA_Web.Data;
using MCBA_Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleHashing;

namespace MCBA_Web.Controllers
{
    // Bonus Material: Implement global authorisation check.
    [AllowAnonymous]
    [Route("/Mcba/SecureLogin")]
    public class LoginController : Controller
    {
        private readonly McbaContext _context;

        public LoginController(McbaContext context) => _context = context;

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(int loginID, string password)
        {
            var login = await _context.Logins.FindAsync(loginID);
            var cust = await _context.Customers.FindAsync(login.CustomerID);
            if (cust.CanLogin == false)
            {
                ModelState.AddModelError("LoginFailed", "This account has been locked.");
                return View(new Login { LoginID = loginID });
            }
            if(login == null || !PBKDF2.Verify(login.PasswordHash, password))
            { 
                ModelState.AddModelError("LoginFailed", "Login failed, please try again.");
                return View(new Login { LoginID = loginID });
            }

            // Login customer.
            HttpContext.Session.SetInt32(nameof(Customer.CustomerID), login.CustomerID);
            HttpContext.Session.SetString(nameof(Customer.Name), login.Customer.Name);
            HttpContext.Session.SetInt32(nameof(login.LoginID), login.LoginID);
            return RedirectToAction("Index", "Customer");
        }

        [Route("LogoutNow")]
        public IActionResult Logout()
        {
            // Logout customer.
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
