using Hw14.Configuration;
using Hw14.Contracts;
using Hw14.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw14.Repositories
{

    public class TransactionRepository : ITransactiionRepository
    {
        private readonly BankDbContext _context;

        public TransactionRepository()
        {
            _context = new BankDbContext();
        }

        public void AddTransaction(Transactiion transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
        }
        public List<Transactiion> GetTransactionsByCardNumber(string cardNumber)
        {
            return _context.Transactions
                .Where(t => t.SourceCardNumber == cardNumber || t.DestinationCardNumber == cardNumber)
                .OrderByDescending(t => t.TransactionDate)
                .ToList();
        }


    }

}

