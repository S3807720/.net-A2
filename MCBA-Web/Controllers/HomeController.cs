using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MCBA_Web.Models;

using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using MCBA_Web.ViewModels;
using Newtonsoft.Json;
using MCBA_Web.Data;
using MCBA_Models.Models;
using System.Linq;

namespace MCBA_Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    }
}