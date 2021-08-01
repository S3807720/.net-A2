using Microsoft.AspNetCore.Mvc;
using MCBA_Models.Models;
using MCBA_WebAPI.Models.DataManager;
using System.Collections.Generic;

namespace MCBA_Web.Controllers
{

        [ApiController]
        [Route("api/[controller]")]
        public class CustomerController : ControllerBase
        {
            private readonly CustomersManager _repo;

            public CustomerController(CustomersManager repo)
            {
                _repo = repo;
            }

            [HttpGet]
            public IEnumerable<Customer> Get()
            {
                return _repo.GetAll();
            }

            [HttpGet("{id}")]
            public Customer Get(int id)
            {
                return _repo.Get(id);
            }

            [HttpPost]
            public void Post([FromBody] Customer customer)
            {
                _repo.Add(customer);
            }

            [HttpPut]
            public void Put([FromBody] Customer customer)
            {
                _repo.Update(customer.CustomerID, customer);
            }

            [HttpDelete("{id}")]
            public long Delete(int id)
            {
                return _repo.Delete(id);
            }
        }

    }

    
