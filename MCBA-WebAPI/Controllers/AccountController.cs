using Microsoft.AspNetCore.Mvc;
using MCBA_Models.Models;
using MCBA_WebAPI.Models.DataManager;
using System.Collections.Generic;

namespace MCBA_Web.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AccountsManager _repo;

        public AccountController(AccountsManager repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IEnumerable<Account> Get()
        {
            return _repo.GetAll();
        }

        [HttpGet("{id}")]
        public Account Get(int id)
        {
            return _repo.Get(id);
        }

        [HttpPost]
        public void Post([FromBody] Account account)
        {
            _repo.Add(account);
        }

        [HttpPut]
        public void Put([FromBody] Account account)
        {
            _repo.Update(account.AccountNumber, account);
        }

        [HttpDelete("{id}")]
        public long Delete(int id)
        {
            return _repo.Delete(id);
        }
    }

}


