using MCBA_WebAPI.Models.Repository;
using MCBA_WebAPI.Data;
using MCBA_Models.Models;
using System.Collections.Generic;
using System.Linq;

namespace MCBA_WebAPI.Models.DataManager
{
    public class LoginManager : IDataRepository<Login, int>
    {
        private readonly McbaContext _context;
        public LoginManager(McbaContext context)
        {
            _context = context;
        }
        public Login Get(int id)
        {
            return _context.Logins.Find(id);
        }

        public IEnumerable<Login> GetAll()
        {
            return _context.Logins.ToList();
        }

        public int Add(Login login)
        {
            _context.Logins.Add(login);
            _context.SaveChanges();

            return login.CustomerID;
        }

        public int Delete(int id)
        {
            _context.Logins.Remove(_context.Logins.Find(id));
            _context.SaveChanges();

            return id;
        }

        public int Update(int id, Login login)
        {
            _context.Update(login);
            _context.SaveChanges();

            return id;
        }
    }
}
