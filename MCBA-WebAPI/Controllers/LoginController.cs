using Microsoft.AspNetCore.Mvc;
using MCBA_Models.Models;
using MCBA_WebAPI.Models.DataManager;
using System.Collections.Generic;

namespace MCBA_Web.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly LoginManager _repo;

        public LoginController(LoginManager repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IEnumerable<Login> Get()
        {
            return _repo.GetAll();
        }

        [HttpGet("{id}")]
        public Login Get(int id)
        {
            return _repo.Get(id);
        }

        [HttpPost]
        public void Post([FromBody] Login login)
        {
            _repo.Add(login);
        }

        [HttpPut]
        public void Put([FromBody] Login login)
        {
            _repo.Update(login.LoginID, login);
        }

        [HttpDelete("{id}")]
        public long Delete(int id)
        {
            return _repo.Delete(id);
        }
    }

}


