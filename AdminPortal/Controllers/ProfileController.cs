using MCBA_Models.Models;
using MCBA_Admin.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MCBA_Web.Filters;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace MCBA_Admin.Controllers
{
    public class ProfileController : Controller
    {

        private readonly IHttpClientFactory _clientFactory;
        private HttpClient Client => _clientFactory.CreateClient("api");
        public ProfileController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        [AuthorizeCustomer]
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var response = await Client.GetAsync($"api/customer/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            var result = await response.Content.ReadAsStringAsync();
            var customer = JsonConvert.DeserializeObject<Customer>(result);
            return View(customer);
        }

        [HttpPost]
        public IActionResult EditProfile(Customer cust)
        {
            if (cust == null)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                var content = JsonContent.Create(cust);
                var response = Client.PutAsync("api/customer", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Customer", new { area = "" });
                }
            }
            return View(cust);
        }
    }
}
