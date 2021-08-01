using MCBA_WebAPI.Models.Repository;
using MCBA_WebAPI.Data;
using MCBA_Models.Models;
using System.Collections.Generic;
using System.Linq;

namespace MCBA_WebAPI.Models.DataManager
{
    public class CustomersManager : IDataRepository<Customer, int>
    {
        private readonly McbaContext _context;
        public CustomersManager(McbaContext context)
        {
            _context = context;
        }
        public Customer Get(int id)
        {
            return _context.Customers.Find(id);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _context.Customers.ToList();
        }

        public int Add(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();

            return customer.CustomerID;
        }

        public int Delete(int id)
        {
            _context.Customers.Remove(_context.Customers.Find(id));
            _context.SaveChanges();

            return id;
        }

        public int Update(int id, Customer customer)
        {
            _context.Update(customer);
            _context.SaveChanges();

            return id;
        }
    }
}
