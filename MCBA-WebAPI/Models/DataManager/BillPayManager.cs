using MCBA_WebAPI.Models.Repository;
using MCBA_WebAPI.Data;
using MCBA_Models.Models;
using System.Collections.Generic;
using System.Linq;

namespace MCBA_WebAPI.Models.DataManager
{
    public class BillPayManager : IDataRepository<BillPay, int>
    {
        private readonly McbaContext _context;
        public BillPayManager(McbaContext context)
        {
            _context = context;
        }
        public BillPay Get(int id)
        {
            return _context.BillPays.Find(id);
        }

        public IEnumerable<BillPay> GetAll()
        {
            return _context.BillPays.ToList();
        }

        public int Add(BillPay bp)
        {
            _context.BillPays.Add(bp);
            _context.SaveChanges();

            return bp.BillPayID;
        }

        public int Delete(int id)
        {
            _context.BillPays.Remove(_context.BillPays.Find(id));
            _context.SaveChanges();

            return id;
        }

        public int Update(int id, BillPay bp)
        {
            _context.Update(bp);
            _context.SaveChanges();

            return id;
        }
    }
}
