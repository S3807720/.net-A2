using Microsoft.AspNetCore.Mvc;
using MCBA_Models.Models;
using MCBA_WebAPI.Models.DataManager;
using System.Collections.Generic;

namespace MCBA_Web.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class BillPayController : ControllerBase
    {
        private readonly BillPayManager _repo;

        public BillPayController(BillPayManager repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IEnumerable<BillPay> Get()
        {
            return _repo.GetAll();
        }

        [HttpGet("{id}")]
        public BillPay Get(int id)
        {
            return _repo.Get(id);
        }

        [HttpPost]
        public void Post([FromBody] BillPay bp)
        {
            _repo.Add(bp);
        }

        [HttpPut]
        public void Put([FromBody] BillPay bp)
        {
            _repo.Update(bp.BillPayID, bp);
        }

        [HttpDelete("{id}")]
        public long Delete(int id)
        {
            return _repo.Delete(id);
        }
    }

}


