using System.Net.Http;
using System.Threading.Tasks;
using MCBA_Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MCBA_Admin.Controllers
{
    // Bonus Material: Implement global authorisation check.
    [AllowAnonymous]
    [Route("/Mcba/SecureLogin")]
    public class LoginController : Controller
    {
        private readonly string PASS = "admin";
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string loginID, string password)
        {
            if(password != PASS || loginID != PASS)
            { 
                ModelState.AddModelError("LoginFailed", "Login failed, please try again.");
                return View(new MCBA_Admin.Models.Login { LoginID = loginID });
            }
            HttpContext.Session.SetString(nameof(Customer.CustomerID), loginID);
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
