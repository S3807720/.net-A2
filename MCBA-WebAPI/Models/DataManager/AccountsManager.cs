﻿using MCBA_WebAPI.Models.Repository;
using MCBA_WebAPI.Data;
using MCBA_Models.Models;
using System.Collections.Generic;
using System.Linq;

namespace MCBA_WebAPI.Models.DataManager
{
    public class AccountsManager : IDataRepository<Account, int>
    {
        private readonly McbaContext _context;
        public AccountsManager(McbaContext context)
        {
            _context = context;
        }
        public Account Get(int id)
        {
            return _context.Accounts.Find(id);
        }

        public IEnumerable<Account> GetAll()
        {
            return _context.Accounts.ToList();
        }

        public int Add(Account account)
        {
            _context.Accounts.Add(account);
            _context.SaveChanges();

            return account.AccountNumber;
        }

        public int Delete(int id)
        {
            _context.Accounts.Remove(_context.Accounts.Find(id));
            _context.SaveChanges();

            return id;
        }

        public int Update(int id, Account account)
        {
            _context.Update(account);
            _context.SaveChanges();

            return id;
        }
    }
}
