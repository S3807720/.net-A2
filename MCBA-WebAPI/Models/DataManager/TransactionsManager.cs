using MCBA_WebAPI.Models.Repository;
using MCBA_WebAPI.Data;
using MCBA_Models.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MCBA_WebAPI.Models.DataManager
{
    public class TransactionsManager : IDataRepository<Transaction, int>
    {
        private readonly McbaContext _context;
        public TransactionsManager(McbaContext context)
        {
            _context = context;
        }
        public Transaction Get(int id)
        {
            return _context.Transactions.Find(id);
        }

        public IEnumerable<Transaction> GetAll()
        {
            return _context.Transactions.ToList();
        }

        public int Add(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return transaction.TransactionID;
        }

        public int Delete(int id)
        {
            _context.Transactions.Remove(_context.Transactions.Find(id));
            _context.SaveChanges();

            return id;
        }

        public int Update(int id, Transaction transaction)
        {
            _context.Update(transaction);
            _context.SaveChanges();

            return id;
        }
    }
}
